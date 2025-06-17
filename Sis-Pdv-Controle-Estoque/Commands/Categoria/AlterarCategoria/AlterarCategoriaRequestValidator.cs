using FluentValidation;

namespace Commands.Categoria.AlterarCategoria
{
    public class AlterarCategoriaRequestValidator : AbstractValidator<AlterarCategoriaRequest>
    {
        public AlterarCategoriaRequestValidator()
        {
            RuleFor(request => request.NomeCategoria).NotEmpty().WithMessage("O nome da categoria é obrigatório.");
            RuleFor(request => request.NomeCategoria).Length(2, 100).WithMessage("O nome da categoria deve ter entre 2 e 100 caracteres.");
            RuleFor(request => request.Id).NotNull().WithMessage("O Id da categoria é obrigatório.");
        }
    }
}
