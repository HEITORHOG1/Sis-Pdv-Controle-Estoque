using FluentValidation;

namespace Commands.Colaborador.RemoverColaborador
{
    public class RemoverColaboradorRequestValidator : AbstractValidator<RemoverColaboradorRequest>
    {
        public RemoverColaboradorRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do colaborador é obrigatório.");
        }
    }
}
