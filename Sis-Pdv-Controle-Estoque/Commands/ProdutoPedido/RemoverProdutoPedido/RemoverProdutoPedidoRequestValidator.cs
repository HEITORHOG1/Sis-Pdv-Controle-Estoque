using FluentValidation;

namespace Commands.ProdutoPedido.RemoverProdutoPedido
{
    public class RemoverProdutoPedidoRequestValidator : AbstractValidator<RemoverProdutoPedidoRequest>
    {
        public RemoverProdutoPedidoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do item do pedido é obrigatório.");
        }
    }
}
