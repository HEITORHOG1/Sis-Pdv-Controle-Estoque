using Commands.Pedido.AdicionarPedido;
using FluentValidation;

namespace AdicionarPedido
{
    public class AdicionarPedidoRequestValidator : AbstractValidator<AdicionarPedidoRequest>
    {
        public AdicionarPedidoRequestValidator()
        {

            RuleFor(request => request.ColaboradorId).NotEmpty().WithMessage("O ID do colaborador é obrigatório.");

            RuleFor(request => request.ClienteId).NotEmpty().WithMessage("O ID do cliente é obrigatório.");

            RuleFor(request => request.Status).InclusiveBetween(0, 1).WithMessage("O Status deve ser 0 (inativo) ou 1 (ativo).");

            RuleFor(request => request.dataDoPedido).NotEmpty().WithMessage("A data do pedido é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data do pedido não pode ser no futuro.");

            RuleFor(request => request.formaPagamento).NotEmpty().WithMessage("A forma de pagamento é obrigatória.");

            RuleFor(request => request.totalPedido).GreaterThan(0).WithMessage("O total do pedido deve ser maior que zero.");
        }
    }

}
