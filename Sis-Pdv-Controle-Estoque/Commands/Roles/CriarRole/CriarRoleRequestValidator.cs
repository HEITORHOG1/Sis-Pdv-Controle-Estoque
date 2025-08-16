using FluentValidation;

namespace Commands.Roles.CriarRole
{
    public class CriarRoleRequestValidator : AbstractValidator<CriarRoleRequest>
    {
        public CriarRoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome da role é obrigatório")
                .MinimumLength(2).WithMessage("Nome da role deve ter pelo menos 2 caracteres")
                .MaximumLength(50).WithMessage("Nome da role deve ter no máximo 50 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descrição da role é obrigatória")
                .MaximumLength(200).WithMessage("Descrição da role deve ter no máximo 200 caracteres");
        }
    }
}