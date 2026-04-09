using FluentValidation;

namespace Commands.Cupom.AlterarCupom
{
    public class AlterarCupomRequestValidator : AbstractValidator<AlterarCupomRequest>
    {
        public AlterarCupomRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id do cupom é obrigatório.");

            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("O Id do pedido é obrigatório.");

            RuleFor(x => x.NumeroSerie)
                .NotEmpty().WithMessage("O número de série é obrigatório.")
                .MaximumLength(50).WithMessage("O número de série deve ter no máximo 50 caracteres.");

            RuleFor(x => x.ChaveAcesso)
                .NotEmpty().WithMessage("A chave de acesso é obrigatória.")
                .MaximumLength(100).WithMessage("A chave de acesso deve ter no máximo 100 caracteres.");

            RuleFor(x => x.ValorTotal)
                .GreaterThan(0).WithMessage("O valor total deve ser maior que zero.");
        }
    }
}
