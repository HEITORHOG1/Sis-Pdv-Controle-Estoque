using FluentValidation;

namespace Commands.Usuarios.AlterarSenha
{
    public class AlterarSenhaRequestValidator : AbstractValidator<AlterarSenhaRequest>
    {
        public AlterarSenhaRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("ID do usuário é obrigatório");

            RuleFor(x => x.SenhaAtual)
                .NotEmpty().WithMessage("Senha atual é obrigatória");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("Nova senha é obrigatória")
                .MinimumLength(6).WithMessage("Nova senha deve ter pelo menos 6 caracteres")
                .MaximumLength(100).WithMessage("Nova senha deve ter no máximo 100 caracteres");

            RuleFor(x => x.ConfirmarNovaSenha)
                .NotEmpty().WithMessage("Confirmação da nova senha é obrigatória")
                .Equal(x => x.NovaSenha).WithMessage("Confirmação da nova senha deve ser igual à nova senha");
        }
    }
}