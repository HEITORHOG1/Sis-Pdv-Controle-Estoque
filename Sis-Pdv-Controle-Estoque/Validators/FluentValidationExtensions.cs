using FluentValidation;

namespace Validators
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(CpfCnpjValidator.IsValidCpf)
                .WithMessage("CPF inválido");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(CpfCnpjValidator.IsValidCnpj)
                .WithMessage("CNPJ inválido");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCpfOrCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(CpfCnpjValidator.IsValidCpfOrCnpj)
                .WithMessage("CPF ou CNPJ inválido");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidBarcode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(s =>
            {
                if (string.IsNullOrWhiteSpace(s)) return false;
                // Primeiro tenta EAN/UPC com dígito verificador
                if (BarcodeValidator.IsValidBarcode(s)) return true;
                // Fallback: aceitamos códigos numéricos entre 8 e 20 dígitos (sem validar dígito verificador)
                var clean = s.Trim();
                return clean.Length >= 8 && clean.Length <= 20 && clean.All(char.IsDigit);
            })
            .WithMessage("Código de barras inválido para formato EAN/UPC");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidInternalCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(BarcodeValidator.IsValidInternalCode)
                .WithMessage("Código interno deve ter entre 6 e 20 caracteres alfanuméricos");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidEmail)
                .WithMessage("Email inválido");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidBrazilianPhone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidBrazilianPhone)
                .WithMessage("Telefone deve ter 10 ou 11 dígitos");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCep<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidCep)
                .WithMessage("CEP deve ter 8 dígitos");
        }

        public static IRuleBuilderOptions<T, decimal> MustBeValidPrice<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidPrice)
                .WithMessage("Preço deve ser um valor positivo com até 2 casas decimais");
        }

        public static IRuleBuilderOptions<T, decimal> MustBeValidQuantity<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidQuantity)
                .WithMessage("Quantidade deve ser um valor positivo");
        }

        public static IRuleBuilderOptions<T, decimal> MustBeValidPercentage<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidPercentage)
                .WithMessage("Percentual deve estar entre 0 e 9999,99");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidName)
                .WithMessage("Nome deve conter apenas letras e espaços");
        }

        public static IRuleBuilderOptions<T, DateTime> MustBeValidPastDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidPastDate)
                .WithMessage("Data não pode ser no futuro");
        }

        public static IRuleBuilderOptions<T, DateTime> MustBeValidBusinessDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Must(BusinessValidator.IsValidBusinessDate)
                .WithMessage("Data deve estar dentro de um período válido para negócios");
        }
    }
}