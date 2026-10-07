using System.Net;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public abstract class CrudMvcController<T>(ICrudService<T> service, ILogger logger) : Controller where T : class, new()
{
    protected ICrudService<T> Service => service;
    protected virtual Task PrepareAsync(T dto) => Task.CompletedTask;
    public async Task<IActionResult> Index(int page = 1)
    {
        try { return View(await service.GetPageAsync(page)); }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        { logger.LogError(ex, "Error cargando listado"); return StatusCode(503, "No se pudo cargar el listado. Revisá que la API y SQL Server estén disponibles."); }
    }
    [HttpGet]
    public async Task<IActionResult> Create(int page = 1)
    {
        var dto = new T();
        await PrepareAsync(dto);
        ViewData["Mode"] = "Create"; ViewData["Page"] = page;
        return PartialView("_Form", dto);
    }
    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] string id, int page = 1)
    {
        try
        {
            var dto = await service.FindAsync(id);
            if (dto is null) return NotFound();
            await PrepareAsync(dto);
            ViewData["Mode"] = "Edit"; ViewData["RecordId"] = id; ViewData["Page"] = page;
            return PartialView("_Form", dto);
        }
        catch (ApiException ex) { return StatusCode((int)ex.Status, ex.Message); }
    }
    [HttpGet]
    public virtual async Task<IActionResult> Details([FromRoute] string id)
    {
        try
        {
            var dto = await service.FindAsync(id);
            return dto is null ? NotFound() : PartialView("_Details", dto);
        }
        catch (ApiException ex) { return StatusCode((int)ex.Status, ex.Message); }
    }
    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] string id, int page = 1)
    {
        try
        {
            var dto = await service.FindAsync(id);
            if (dto is null) return NotFound();
            ViewData["RecordId"] = id; ViewData["Page"] = page;
            return PartialView("_Delete", dto);
        }
        catch (ApiException ex) { return StatusCode((int)ex.Status, ex.Message); }
    }
    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(T dto, int page = 1) => SaveAsync(dto, null, page);
    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit([FromRoute] string id, T dto, int page = 1) => SaveAsync(dto, id, page);
    private async Task<IActionResult> SaveAsync(T dto, string? id, int page)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (id is null) await service.CreateAsync(dto); else await service.UpdateAsync(id, dto);
                return Json(new { saved = true, redirect = Url.Action("Index", new { page }) });
            }
            catch (ApiException ex)
            {
                if (ex.Errors.Count == 0) ModelState.AddModelError("", ex.Message);
                foreach (var error in ex.Errors) foreach (var message in error.Value) ModelState.AddModelError(error.Key, message);
            }
            catch (HttpRequestException ex) { logger.LogError(ex, "API no disponible"); ModelState.AddModelError("", "La API no está disponible. Intentá de nuevo."); }
        }
        await PrepareAsync(dto);
        ViewData["Mode"] = id is null ? "Create" : "Edit";
        ViewData["RecordId"] = id; ViewData["Page"] = page;
        // Never re-render a submitted password into the modal.
        if (dto is PAW.Models.DTO.UserDTO user) user.NewPassword = null;
        if (ModelState.ContainsKey("NewPassword"))
            ModelState.SetModelValue("NewPassword", string.Empty, string.Empty);
        Response.StatusCode = 422;
        return PartialView("_Form", dto);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed([FromRoute] string id, int page = 1)
    {
        try
        {
            await service.DeleteAsync(id);
            return Json(new { saved = true, redirect = Url.Action("Index", new { page }) });
        }
        catch (ApiException ex) { return StatusCode((int)ex.Status, ex.Message); }
        catch (HttpRequestException ex) { logger.LogError(ex, "API no disponible"); return StatusCode(503, "La API no está disponible. Intentá de nuevo."); }
    }
}
