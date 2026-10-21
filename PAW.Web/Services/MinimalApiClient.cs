namespace PAW.Web.Services;

// Tipo distinto para no modificar la dirección de ApiClient usada por las demás entidades.
public class MinimalApiClient(HttpClient client) : ApiClient(client)
{
}
