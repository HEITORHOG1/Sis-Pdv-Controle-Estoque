using FluentValidation;

namespace Commands.Cliente.AdicionarCliente
{
    public class AdicionarClienteRequestValidator : AbstractValidator<AdicionarClienteRequest>
    {
        public AdicionarClienteRequestValidator()
        {
            RuleFor(request => request.CpfCnpj).NotEmpty().WithMessage("O CPF/CNPJ é obrigatório.");
            RuleFor(request => request.CpfCnpj).Length(11, 14).WithMessage("O CPF deve ter 11 dígitos e o CNPJ 14 dígitos.");

            RuleFor(request => request.TipoCliente).NotEmpty().WithMessage("O tipo de cliente é obrigatório.");
            RuleFor(request => request.TipoCliente).Must(TipoClienteValido).WithMessage("O tipo de cliente deve ser 'Físico' ou 'Jurídico'.");
        }

        private bool TipoClienteValido(string tipoCliente)
        {
            return tipoCliente.Equals("Físico", StringComparison.OrdinalIgnoreCase) || tipoCliente.Equals("Jurídico", StringComparison.OrdinalIgnoreCase);
        }
    }
}
