using FluentValidation;

namespace Commands.Categoria.RemoverCategoria
{
    public class RemoverCategoriaRequestValidator : AbstractValidator<RemoverCategoriaRequest>
    {
        public RemoverCategoriaRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O Id da categoria é obrigatório.");
        }
    }
}
