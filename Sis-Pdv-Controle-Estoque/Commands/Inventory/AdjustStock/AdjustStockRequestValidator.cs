using FluentValidation;

namespace Commands.Inventory.AdjustStock
{
    public class AdjustStockRequestValidator : AbstractValidator<AdjustStockRequest>
    {
        public AdjustStockRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ID do produto é obrigatório");

            RuleFor(x => x.NewQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantidade deve ser maior ou igual a zero");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Motivo do ajuste é obrigatório")
                .MaximumLength(500)
                .WithMessage("Motivo deve ter no máximo 500 caracteres");

            RuleFor(x => x.ReferenceDocument)
                .MaximumLength(100)
                .WithMessage("Documento de referência deve ter no máximo 100 caracteres");
        }
    }
}