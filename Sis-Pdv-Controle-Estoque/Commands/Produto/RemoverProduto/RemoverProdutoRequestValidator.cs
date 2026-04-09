using FluentValidation;

namespace Commands.Produto.RemoverProduto
{
    public class RemoverProdutoRequestValidator : AbstractValidator<RemoverProdutoRequest>
    {
        public RemoverProdutoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do produto é obrigatório.");
        }
    }
}
