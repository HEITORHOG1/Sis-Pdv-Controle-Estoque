namespace Sis.Pdv.Blazor.Configuration;

/// <summary>
/// Configurações de conexão com a API REST.
/// </summary>
public sealed class ApiSettings
{
    public const string SectionName = "ApiSettings";
    public const string HttpClientName = "PdvApi";

    public string BaseUrl { get; init; } = "http://localhost:7003";
    public int TimeoutSeconds { get; init; } = 15;
}
