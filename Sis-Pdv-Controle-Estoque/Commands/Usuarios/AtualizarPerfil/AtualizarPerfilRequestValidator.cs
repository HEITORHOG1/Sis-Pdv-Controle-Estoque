using FluentValidation;

namespace Commands.Usuarios.AtualizarPerfil
{
    public class AtualizarPerfilRequestValidator : AbstractValidator<AtualizarPerfilRequest>
    {
        public AtualizarPerfilRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("ID do usuário é obrigatório");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");
        }
    }
}