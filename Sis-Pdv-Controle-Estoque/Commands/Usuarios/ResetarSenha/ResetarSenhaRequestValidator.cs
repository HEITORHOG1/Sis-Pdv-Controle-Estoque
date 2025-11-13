using FluentValidation;

namespace Commands.Usuarios.ResetarSenha
{
    public class ResetarSenhaRequestValidator : AbstractValidator<ResetarSenhaRequest>
    {
        public ResetarSenhaRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("Id do usuário é obrigatório");

            RuleFor(x => x.NovaSenha)
                .NotEmpty()
                .WithMessage("Nova senha é obrigatória")
                .MinimumLength(6)
                .WithMessage("Nova senha deve ter no mínimo 6 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Nova senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número");

            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty()
                .WithMessage("Confirmação de senha é obrigatória")
                .Equal(x => x.NovaSenha)
                .WithMessage("Confirmação de senha deve ser igual à nova senha");
        }
    }
}