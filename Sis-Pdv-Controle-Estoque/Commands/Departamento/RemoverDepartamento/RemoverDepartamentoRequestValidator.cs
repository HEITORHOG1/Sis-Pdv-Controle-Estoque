using FluentValidation;

namespace Commands.Departamento.RemoverDepartamento
{
    public class RemoverDepartamentoRequestValidator : AbstractValidator<RemoverDepartamentoRequest>
    {
        public RemoverDepartamentoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do departamento é obrigatório.");
        }
    }
}
