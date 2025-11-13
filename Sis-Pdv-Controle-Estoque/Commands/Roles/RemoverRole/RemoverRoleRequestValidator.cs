using FluentValidation;

namespace Commands.Roles.RemoverRole
{
    public class RemoverRoleRequestValidator : AbstractValidator<RemoverRoleRequest>
    {
        public RemoverRoleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id da role é obrigatório");
        }
    }
}