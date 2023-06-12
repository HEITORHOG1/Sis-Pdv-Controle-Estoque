using FluentValidation;

namespace Commands.Categoria.AdicionarCategoria
{
    public class AdicionarCategoriaValidator : AbstractValidator<AdicionarCategoriaRequest>
    {
        public AdicionarCategoriaValidator()
        {
            RuleFor(categoria => categoria.NomeCategoria).NotEmpty().WithMessage("O nome da categoria é obrigatório.");
            RuleFor(categoria => categoria.NomeCategoria).Length(2, 100).WithMessage("O nome da categoria deve ter entre 2 e 100 caracteres.");
        }
    }
}
