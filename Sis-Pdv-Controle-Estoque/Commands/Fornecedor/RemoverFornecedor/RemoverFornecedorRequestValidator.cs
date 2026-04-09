using FluentValidation;

namespace Commands.Fornecedor.RemoverFornecedor
{
    public class RemoverFornecedorRequestValidator : AbstractValidator<RemoverFornecedorRequest>
    {
        public RemoverFornecedorRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do fornecedor é obrigatório.");
        }
    }
}
