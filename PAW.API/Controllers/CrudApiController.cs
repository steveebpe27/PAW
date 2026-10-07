using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;
namespace PAW.API.Controllers;
// Shared actions are inherited by the eleven concrete [ApiController] classes.
public abstract class CrudApiController<TEntity, TDto>(ICrudRepository<TEntity> repository, ILogger logger)
    : ControllerBase where TEntity : class, new() where TDto : class
{
    protected abstract TDto Map(TEntity entity);
    protected abstract void Apply(TDto dto, TEntity entity);
    protected abstract string Key(TEntity entity);
    protected abstract string Label(TEntity entity);
    protected virtual Task<Dictionary<string, string[]>> ValidateAsync(TDto dto, bool isNew) =>
        Task.FromResult(new Dictionary<string, string[]>());
    protected virtual Task<bool> HasReferencesAsync(TEntity entity) => Task.FromResult(false);
    [HttpGet]
    public async Task<PageResult<TDto>> GetAll(int page = 1, int pageSize = 25)
    {
        var result = await repository.ReadPageAsync(page, pageSize);
        return new PageResult<TDto> { Page = result.Page, PageSize = result.PageSize, TotalCount = result.TotalCount,
            Items = result.Items.Select(Map).ToList() };
    }
    [HttpGet("options")]
    public async Task<IEnumerable<LookupOption>> Options() =>
        (await repository.ReadAsync()).Select(x => new LookupOption(Key(x), Label(x)));
    [HttpGet("{id}")]
    public async Task<ActionResult<TDto>> GetById(string id)
    {
        var entity = await repository.FindAsync(id);
        return entity is null ? NotFound() : Ok(Map(entity));
    }
    [HttpPost]
    public async Task<ActionResult<TDto>> Create([FromBody] TDto dto)
    {
        var errors = await ValidateAsync(dto, true);
        if (errors.Count != 0) return BadRequest(new ValidationProblemDetails(errors));
        var entity = new TEntity();
        Apply(dto, entity);
        try { await repository.SaveAsync(entity, true); }
        catch (DbUpdateException ex) { return DatabaseConflict(ex); }
        return Created($"{Request.Path}/{Key(entity)}", Map(entity));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TDto dto)
    {
        var entity = await repository.FindAsync(id);
        if (entity is null) return NotFound();
        var errors = await ValidateAsync(dto, false);
        if (errors.Count != 0) return BadRequest(new ValidationProblemDetails(errors));
        Apply(dto, entity);
        try { await repository.SaveAsync(entity, false); }
        catch (DbUpdateConcurrencyException) { return Conflict(new ProblemDetails { Title = "El registro cambió o fue eliminado. Recargá el listado." }); }
        catch (DbUpdateException ex) { return DatabaseConflict(ex); }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entity = await repository.FindAsync(id);
        if (entity is null) return NotFound();
        if (await HasReferencesAsync(entity))
            return Conflict(new ProblemDetails { Title = "El registro está asociado a otros datos. Cambiá esas referencias antes de eliminarlo." });
        try { await repository.DeleteAsync(entity); }
        catch (DbUpdateConcurrencyException) { return Conflict(new ProblemDetails { Title = "El registro ya cambió. Recargá el listado." }); }
        catch (DbUpdateException ex) { return DatabaseConflict(ex); }
        return NoContent();
    }
    private ObjectResult DatabaseConflict(DbUpdateException ex)
    {
        logger.LogWarning(ex, "No se pudo guardar {Entity}", typeof(TEntity).Name);
        return Conflict(new ProblemDetails { Title = "No se pudo guardar el cambio. Revisá las referencias y restricciones; un registro asociado puede impedir eliminarlo." });
    }
}
