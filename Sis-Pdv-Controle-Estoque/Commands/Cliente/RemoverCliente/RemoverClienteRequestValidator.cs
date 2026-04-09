using FluentValidation;

namespace Commands.Cliente.RemoverCliente
{
    public class RemoverClienteRequestValidator : AbstractValidator<RemoverClienteRequest>
    {
        public RemoverClienteRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do cliente é obrigatório.");
        }
    }
}
