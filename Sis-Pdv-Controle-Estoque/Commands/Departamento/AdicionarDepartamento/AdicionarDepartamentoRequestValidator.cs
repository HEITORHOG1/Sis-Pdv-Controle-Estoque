using FluentValidation;
using Validators;

namespace Commands.Departamento.AdicionarDepartamento
{
    public class AdicionarDepartamentoRequestValidator : AbstractValidator<AdicionarDepartamentoRequest>
    {
        public AdicionarDepartamentoRequestValidator()
        {
            RuleFor(request => request.NomeDepartamento)
                .NotEmpty().WithMessage("O nome do departamento é obrigatório.")
                .Length(2, 100).WithMessage("O nome do departamento deve ter entre 2 e 100 caracteres.")
                .MustBeValidName();
        }
    }
}
