using FluentValidation;

namespace Commands.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioRequestValidator : AbstractValidator<RegistrarUsuarioRequest>
    {
        public RegistrarUsuarioRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login é obrigatório")
                .MinimumLength(3).WithMessage("Login deve ter pelo menos 3 caracteres")
                .MaximumLength(50).WithMessage("Login deve ter no máximo 50 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
                .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres");

            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty().WithMessage("Confirmação de senha é obrigatória")
                .Equal(x => x.Senha).WithMessage("Confirmação de senha deve ser igual à senha");
        }
    }
}