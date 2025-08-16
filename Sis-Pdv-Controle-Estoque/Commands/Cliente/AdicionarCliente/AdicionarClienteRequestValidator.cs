using FluentValidation;
using Validators;

namespace Commands.Cliente.AdicionarCliente
{
    public class AdicionarClienteRequestValidator : AbstractValidator<AdicionarClienteRequest>
    {
        public AdicionarClienteRequestValidator()
        {
            RuleFor(request => request.CpfCnpj)
                .NotEmpty().WithMessage("O CPF/CNPJ é obrigatório.")
                .MustBeValidCpfOrCnpj();

            RuleFor(request => request.TipoCliente)
                .NotEmpty().WithMessage("O tipo de cliente é obrigatório.")
                .Must(TipoClienteValido).WithMessage("O tipo de cliente deve ser 'Físico' ou 'Jurídico'.");

            // Cross-field validation: CPF for Físico, CNPJ for Jurídico
            RuleFor(request => request)
                .Must(ValidarTipoDocumento)
                .WithMessage("CPF deve ser usado para pessoa física e CNPJ para pessoa jurídica.")
                .WithName("CpfCnpj");
        }

        private bool TipoClienteValido(string tipoCliente)
        {
            return tipoCliente.Equals("Físico", StringComparison.OrdinalIgnoreCase) || 
                   tipoCliente.Equals("Jurídico", StringComparison.OrdinalIgnoreCase);
        }

        private bool ValidarTipoDocumento(AdicionarClienteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CpfCnpj) || string.IsNullOrWhiteSpace(request.TipoCliente))
                return true; // Let other validators handle empty values

            var cleanDocument = request.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            var isFisico = request.TipoCliente.Equals("Físico", StringComparison.OrdinalIgnoreCase);

            return isFisico ? cleanDocument.Length == 11 : cleanDocument.Length == 14;
        }
    }
}
