using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.ViewComponents;

public class ExchangeRateViewComponent(
    IExchangeRateService service,
    ILogger<ExchangeRateViewComponent> logger) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new ExchangeRateViewModel();
        try
        {
            model.TipoCambio = await service.GetExchangeRateAsync(HttpContext.RequestAborted);
        }
        catch (OperationCanceledException) when (!HttpContext.RequestAborted.IsCancellationRequested)
        {
            logger.LogWarning("La consulta del tipo de cambio excedió el tiempo de espera.");
            model.ErrorMessage = "No se pudo consultar el tipo de cambio a tiempo. Intentá nuevamente al recargar la página.";
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException)
        {
            logger.LogWarning(ex, "No se pudo obtener el tipo de cambio de Hacienda.");
            model.ErrorMessage = "El tipo de cambio no está disponible en este momento. Intentá nuevamente al recargar la página.";
        }

        return View("~/Views/Shared/ExchangeRate.cshtml", model);
    }
}
