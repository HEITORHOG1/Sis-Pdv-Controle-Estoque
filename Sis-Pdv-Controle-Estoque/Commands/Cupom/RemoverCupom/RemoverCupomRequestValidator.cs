using FluentValidation;

namespace Commands.Cupom.RemoverCupom
{
    public class RemoverCupomRequestValidator : AbstractValidator<RemoverCupomRequest>
    {
        public RemoverCupomRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id do cupom é obrigatório.");
        }
    }
}
