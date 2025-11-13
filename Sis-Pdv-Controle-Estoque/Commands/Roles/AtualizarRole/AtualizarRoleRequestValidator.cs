using FluentValidation;

namespace Commands.Roles.AtualizarRole
{
    public class AtualizarRoleRequestValidator : AbstractValidator<AtualizarRoleRequest>
    {
        public AtualizarRoleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id da role é obrigatório");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome da role é obrigatório")
                .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Descrição deve ter no máximo 500 caracteres");
        }
    }
}