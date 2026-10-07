using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PAW.Models.DTO;
namespace PAW.Web.Services;
public class ApiException(HttpStatusCode status, string message, Dictionary<string, string[]>? errors = null) : Exception(message)
{
    public HttpStatusCode Status { get; } = status;
    public Dictionary<string, string[]> Errors { get; } = errors ?? [];
}
public class ApiClient(HttpClient client)
{
    public async Task<T?> GetAsync<T>(string path)
    {
        using var response = await client.GetAsync(path);
        await EnsureAsync(response);
        return await response.Content.ReadFromJsonAsync<T>();
    }
    public async Task SendAsync<T>(HttpMethod method, string path, T? body)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body is not null) request.Content = JsonContent.Create(body);
        using var response = await client.SendAsync(request);
        await EnsureAsync(response);
    }
    private static async Task EnsureAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var message = response.StatusCode == HttpStatusCode.NotFound ? "El registro ya no existe." : "No se pudo completar la operación.";
        var errors = new Dictionary<string, string[]>();
        try
        {
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (doc.RootElement.TryGetProperty("title", out var title)) message = title.GetString() ?? message;
            if (doc.RootElement.TryGetProperty("errors", out var validation))
                errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(validation.GetRawText()) ?? [];
        }
        catch (JsonException) { }
        throw new ApiException(response.StatusCode, message, errors);
    }
}
public interface ICrudService<T>
{
    Task<PageResult<T>> GetPageAsync(int page);
    Task<T?> FindAsync(string id);
    Task CreateAsync(T dto);
    Task UpdateAsync(string id, T dto);
    Task DeleteAsync(string id);
}
public class CrudService<T>(ApiClient client, string path) : ICrudService<T> where T : class
{
    protected ApiClient Client => client;
    public async Task<PageResult<T>> GetPageAsync(int page) => await client.GetAsync<PageResult<T>>($"{path}?page={page}&pageSize=25") ?? new();
    public Task<T?> FindAsync(string id) => client.GetAsync<T>($"{path}/{Uri.EscapeDataString(id)}");
    public Task CreateAsync(T dto) => client.SendAsync(HttpMethod.Post, path, dto);
    public Task UpdateAsync(string id, T dto) => client.SendAsync(HttpMethod.Put, $"{path}/{Uri.EscapeDataString(id)}", dto);
    public Task DeleteAsync(string id) => client.SendAsync<object>(HttpMethod.Delete, $"{path}/{Uri.EscapeDataString(id)}", null);
}
public class LookupService(ApiClient client)
{
    public async Task<List<LookupOption>> GetAsync(string entity) => await client.GetAsync<List<LookupOption>>($"{entity}/options") ?? [];
}
