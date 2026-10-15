using PAW.Web.Models;

namespace PAW.Web.Services;

public interface IExchangeRateService
{
    Task<TipoCambioResponse> GetExchangeRateAsync(CancellationToken ct = default);
}

public class ExchangeRateService(TipoCambioClient client) : IExchangeRateService
{
    public Task<TipoCambioResponse> GetExchangeRateAsync(CancellationToken ct = default)
        => client.GetTipoCambioAsync(ct);
}
