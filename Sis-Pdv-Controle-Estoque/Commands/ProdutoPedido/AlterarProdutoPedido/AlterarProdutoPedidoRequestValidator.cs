using FluentValidation;

namespace Commands.ProdutoPedido.AlterarProdutoPedido
{
    public class AlterarProdutoPedidoRequestValidator : AbstractValidator<AlterarProdutoPedidoRequest>
    {
        public AlterarProdutoPedidoRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(request => request.PedidoId).NotEmpty().WithMessage("O ID do pedido é obrigatório.");

            RuleFor(request => request.ProdutoId).NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(request => request.CodBarras).NotEmpty().WithMessage("O código de barras é obrigatório.");

            RuleFor(request => request.QuantidadeItemPedido)
                .GreaterThan(0)
                .WithMessage("A quantidade do item do pedido deve ser maior que zero.");

            RuleFor(request => request.TotalProdutoPedido)
            .GreaterThanOrEqualTo(0)
                .WithMessage("O total do produto do pedido deve ser maior ou igual a zero.");
        }
    }
}
