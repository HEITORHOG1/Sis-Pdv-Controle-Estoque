namespace Sis_Pdv_Controle_Estoque_API.Configuration
{
    public class SefazConfiguration
    {
        public const string SectionName = "Sefaz";

        public string Environment { get; set; } = "Homologacao"; // Homologacao ou Producao
        public string UF { get; set; } = "SP";
        public string CNPJ { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public EnderecoConfiguration Endereco { get; set; } = new();
        public CertificadoConfiguration Certificado { get; set; } = new();
        public WebServiceConfiguration WebService { get; set; } = new();
        public ContingenciaConfiguration Contingencia { get; set; } = new();
        public ConfiguracoesGeraisConfiguration Configuracoes { get; set; } = new();
    }

    public class EnderecoConfiguration
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string CodigoMunicipio { get; set; } = string.Empty;
        public string NomeMunicipio { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
    }

    public class CertificadoConfiguration
    {
        public string Arquivo { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class WebServiceConfiguration
    {
        public int Ambiente { get; set; } = 2; // 1 = Produção, 2 = Homologação
        public string UF { get; set; } = "SP";
        public string Modelo { get; set; } = "65"; // 65 = NFC-e
        public string Versao { get; set; } = "4.00";
    }

    public class ContingenciaConfiguration
    {
        public bool Habilitada { get; set; } = true;
        public int TipoEmissao { get; set; } = 9; // 9 = Contingência off-line
        public string JustificativaContingencia { get; set; } = "Problemas técnicos";
    }

    public class ConfiguracoesGeraisConfiguration
    {
        public bool SalvarXmlEnvio { get; set; } = true;
        public bool SalvarXmlRetorno { get; set; } = true;
        public int TimeoutWebService { get; set; } = 30000;
        public int TentativasEnvio { get; set; } = 3;
        public int IntervaloTentativas { get; set; } = 5000;
    }
}