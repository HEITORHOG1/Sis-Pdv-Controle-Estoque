using Commands.ProdutoPedido.AlterarProdutoPedido;
using FluentValidation;

namespace AlterarProdutoPedido
{
    public class AlterarProdutoPedidoRequestValidator : AbstractValidator<AlterarProdutoPedidoRequest>
    {
        public AlterarProdutoPedidoRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(request => request.PedidoId).NotEmpty().WithMessage("O ID do pedido é obrigatório.");

            RuleFor(request => request.ProdutoId).NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(request => request.codBarras).NotEmpty().WithMessage("O código de barras é obrigatório.");

            RuleFor(request => request.quantidadeItemPedido)
                .GreaterThan(0)
                .WithMessage("A quantidade do item do pedido deve ser maior que zero.");

            RuleFor(request => request.totalProdutoPedido)
            .GreaterThanOrEqualTo(0)
                .WithMessage("O total do produto do pedido deve ser maior ou igual a zero.");
        }
    }
}
