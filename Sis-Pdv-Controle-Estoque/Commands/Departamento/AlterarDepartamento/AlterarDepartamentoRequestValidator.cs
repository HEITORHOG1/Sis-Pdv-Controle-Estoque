using FluentValidation;
using Validators;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoRequestValidator : AbstractValidator<AlterarDepartamentoRequest>
    {
        public AlterarDepartamentoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id do departamento é obrigatório.");

            RuleFor(request => request.NomeDepartamento)
                .NotEmpty().WithMessage("O nome do departamento é obrigatório.")
                .Length(2, 100).WithMessage("O nome do departamento deve ter entre 2 e 100 caracteres.")
                .MustBeValidName();
        }
    }
}
