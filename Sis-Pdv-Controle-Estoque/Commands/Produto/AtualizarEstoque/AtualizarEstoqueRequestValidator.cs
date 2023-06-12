using Commands.Produto.AtualizarEstoque;
using FluentValidation;

namespace AtualizarEstoque
{
    public class AtualizarEstoqueRequestValidator : AbstractValidator<AtualizarEstoqueRequest>
    {
        public AtualizarEstoqueRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(request => request.quatidadeEstoqueProduto)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade em estoque deve ser maior ou igual a zero.");
        }
    }
}
