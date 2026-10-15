using System.Net.Http.Json;
using System.Text.Json;
using PAW.Web.Models;

namespace PAW.Web.Services;

public class TipoCambioClient(HttpClient http)
{
    public async Task<TipoCambioResponse> GetTipoCambioAsync(CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "indicadores/tc");
        using var response = await http.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TipoCambioResponse>(cancellationToken: ct);
        if (result?.Dolar?.Compra is null || result.Dolar.Venta is null || result.Euro is null ||
            result.Dolar.Compra.Fecha == default || result.Dolar.Venta.Fecha == default || result.Euro.Fecha == default ||
            result.Dolar.Compra.Valor <= 0 || result.Dolar.Venta.Valor <= 0 ||
            result.Euro.Dolares <= 0 || result.Euro.Colones <= 0)
        {
            throw new JsonException("La respuesta de Hacienda no contiene un tipo de cambio válido.");
        }
        return result;
    }
}
