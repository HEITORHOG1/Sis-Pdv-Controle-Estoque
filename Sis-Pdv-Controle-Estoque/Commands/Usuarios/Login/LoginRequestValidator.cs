using FluentValidation;

namespace Commands.Usuarios.Login
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Login é obrigatório")
                .MaximumLength(100)
                .WithMessage("Login deve ter no máximo 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Senha é obrigatória")
                .MinimumLength(6)
                .WithMessage("Senha deve ter no mínimo 6 caracteres");
        }
    }
}