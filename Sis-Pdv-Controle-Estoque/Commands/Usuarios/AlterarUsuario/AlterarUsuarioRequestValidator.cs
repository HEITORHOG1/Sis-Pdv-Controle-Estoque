using FluentValidation;

namespace Commands.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioRequestValidator : AbstractValidator<AlterarUsuarioRequest>
    {
        public AlterarUsuarioRequestValidator()
        {
            RuleFor(request => request.IdLogin).NotEmpty().WithMessage("O ID de login é obrigatório.");

            RuleFor(request => request.Login).NotEmpty().WithMessage("O login é obrigatório.");

            RuleFor(request => request.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
