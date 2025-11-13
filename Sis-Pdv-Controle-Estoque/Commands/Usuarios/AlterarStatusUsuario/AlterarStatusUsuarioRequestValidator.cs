using FluentValidation;

namespace Commands.Usuarios.AlterarStatusUsuario
{
    public class AlterarStatusUsuarioRequestValidator : AbstractValidator<AlterarStatusUsuarioRequest>
    {
        public AlterarStatusUsuarioRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("Id do usuário é obrigatório");

            RuleFor(x => x.Motivo)
                .MaximumLength(500)
                .WithMessage("Motivo deve ter no máximo 500 caracteres");
        }
    }
}