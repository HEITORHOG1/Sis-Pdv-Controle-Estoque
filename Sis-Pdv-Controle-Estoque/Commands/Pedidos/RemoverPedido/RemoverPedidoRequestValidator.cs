using FluentValidation;

namespace Commands.Pedidos.RemoverPedido
{
    public class RemoverPedidoRequestValidator : AbstractValidator<RemoverPedidoRequest>
    {
        public RemoverPedidoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do pedido é obrigatório.");
        }
    }
}
