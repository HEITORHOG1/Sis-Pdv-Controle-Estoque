using FluentValidation;
using Validators;

namespace Commands.Pedidos.AdicionarPedido
{
    public class AdicionarPedidoRequestValidator : AbstractValidator<AdicionarPedidoRequest>
    {
        private readonly string[] _formasPagamentoValidas = 
        {
            "Dinheiro", "Cartão de Crédito", "Cartão de Débito", "PIX", "Boleto", "Transferência",
            "Cartao de Credito", "Cartao de Debito", "Transferencia"
        };

        public AdicionarPedidoRequestValidator()
        {
            RuleFor(request => request.ColaboradorId)
                .NotEmpty().WithMessage("O ID do colaborador é obrigatório.");

            // ClienteId can be null for anonymous sales
            RuleFor(request => request.ClienteId)
                .NotEqual(Guid.Empty).WithMessage("ID do cliente inválido.")
                .When(request => request.ClienteId.HasValue);

            RuleFor(request => request.Status)
                .InclusiveBetween(0, 2).WithMessage("O Status deve ser 0 (pendente), 1 (finalizado) ou 2 (cancelado).");

            RuleFor(request => request.DataDoPedido)
                .NotEmpty().WithMessage("A data do pedido é obrigatória.")
                .MustBeValidBusinessDate()
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data do pedido não pode ser no futuro.")
                .GreaterThan(DateTime.Now.AddDays(-30)).WithMessage("A data do pedido não pode ser muito antiga.");

            RuleFor(request => request.FormaPagamento)
                .NotEmpty().WithMessage("A forma de pagamento é obrigatória.")
                .Must(BeValidPaymentMethod).WithMessage($"Forma de pagamento deve ser uma das seguintes: {string.Join(", ", _formasPagamentoValidas)}");

            RuleFor(request => request.TotalPedido)
                .MustBeValidPrice()
                .GreaterThan(0).WithMessage("O total do pedido deve ser maior que zero.")
                .LessThan(100000).WithMessage("Total do pedido muito alto, verifique o valor.");

            RuleFor(request => request.Id)
                .NotEqual(Guid.Empty).WithMessage("ID do pedido inválido.")
                .When(request => request.Id != Guid.Empty);
        }

        private bool BeValidPaymentMethod(string FormaPagamento)
        {
            return _formasPagamentoValidas.Contains(FormaPagamento, StringComparer.OrdinalIgnoreCase);
        }
    }
}
