using Commands.Categoria.AlterarCategoria;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlterarCategoria
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
