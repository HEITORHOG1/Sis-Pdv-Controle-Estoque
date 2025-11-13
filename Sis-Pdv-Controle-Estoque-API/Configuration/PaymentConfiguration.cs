namespace Sis_Pdv_Controle_Estoque_API.Configuration
{
    public class PaymentConfiguration
    {
        public const string SectionName = "Payment";

        public string DefaultProcessor { get; set; } = "Mock";
        public Dictionary<string, PaymentProcessorConfiguration> Processors { get; set; } = new();
        public PaymentValidationConfiguration Validation { get; set; } = new();
        public PaymentTimeoutConfiguration Timeout { get; set; } = new();
    }

    public class PaymentProcessorConfiguration
    {
        public bool Enabled { get; set; }
        public bool SimulateDelay { get; set; }
        public int DelayMs { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string MerchantId { get; set; } = string.Empty;
        public string MerchantKey { get; set; } = string.Empty;
        public string Environment { get; set; } = "sandbox";
    }

    public class PaymentValidationConfiguration
    {
        public bool ValidateCardNumber { get; set; } = true;
        public bool ValidateExpiryDate { get; set; } = true;
        public bool ValidateSecurityCode { get; set; } = true;
        public int MaxInstallments { get; set; } = 12;
    }

    public class PaymentTimeoutConfiguration
    {
        public int CreditCard { get; set; } = 30000;
        public int DebitCard { get; set; } = 20000;
        public int Pix { get; set; } = 10000;
    }
}