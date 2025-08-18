using FluentValidation;
using Validators;

namespace Commands.Categoria.AdicionarCategoria
{
    public class AdicionarCategoriaRequestValidator : AbstractValidator<AdicionarCategoriaRequest>
    {
        public AdicionarCategoriaRequestValidator()
        {
            RuleFor(request => request.NomeCategoria)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .Length(2, 50).WithMessage("O nome da categoria deve ter entre 2 e 50 caracteres.")
                .MustBeValidName();
        }
    }
}