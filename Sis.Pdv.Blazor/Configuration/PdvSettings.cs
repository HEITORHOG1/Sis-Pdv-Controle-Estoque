namespace Sis.Pdv.Blazor.Configuration;

/// <summary>
/// Configurações operacionais do PDV, carregadas via IOptions pattern.
/// </summary>
public sealed class PdvSettings
{
    public const string SectionName = "PdvSettings";

    public string NomeLoja { get; init; } = "LOJA MODELO LTDA";
    public string NumeroCaixa { get; init; } = "001";
    public string Serie { get; init; } = "1";
    public string Ambiente { get; init; } = "HOMOLOGAÇÃO";
    public string Versao { get; init; } = "2.0.0";
    public bool SolicitarCpfCliente { get; init; }
    public bool ImprimirCupomAutomatico { get; init; } = true;
    public bool ExigirAutorizacaoCancelamento { get; init; }
    public bool HabilitarSons { get; init; } = true;
    public bool UsarGavetaSimulada { get; init; } = true;
    public int TempoTelaObrigadoSegundos { get; init; } = 3;
}
