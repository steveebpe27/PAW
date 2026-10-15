using System.Text.Json.Serialization;

namespace PAW.Web.Models;

public class TipoCambioResponse
{
    [JsonRequired, JsonPropertyName("dolar")]
    public Dolar Dolar { get; set; } = new();

    [JsonRequired, JsonPropertyName("euro")]
    public Euro Euro { get; set; } = new();
}

public class Dolar
{
    [JsonRequired, JsonPropertyName("compra")]
    public ValorFecha Compra { get; set; } = new();

    [JsonRequired, JsonPropertyName("venta")]
    public ValorFecha Venta { get; set; } = new();
}

public class ValorFecha
{
    [JsonRequired, JsonPropertyName("fecha")]
    public DateOnly Fecha { get; set; }

    [JsonRequired, JsonPropertyName("valor")]
    public decimal Valor { get; set; }
}

public class Euro
{
    [JsonRequired, JsonPropertyName("fecha")]
    public DateOnly Fecha { get; set; }

    [JsonRequired, JsonPropertyName("dolares")]
    public decimal Dolares { get; set; }

    [JsonRequired, JsonPropertyName("colones")]
    public decimal Colones { get; set; }
}
