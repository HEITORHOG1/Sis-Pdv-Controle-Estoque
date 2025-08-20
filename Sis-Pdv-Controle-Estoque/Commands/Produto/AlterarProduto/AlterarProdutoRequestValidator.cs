using FluentValidation;
using Validators;

namespace Commands.Produto.AlterarProduto
{
    public class AlterarProdutoRequestValidator : AbstractValidator<AlterarProdutoRequest>
    {
        public AlterarProdutoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(request => request.codBarras)
                .NotEmpty().WithMessage("O código de barras é obrigatório.")
                .MustBeValidBarcode()
                .When(request => !string.IsNullOrWhiteSpace(request.codBarras) && request.codBarras.All(char.IsDigit))
                .WithMessage("Código de barras inválido para formato EAN/UPC");

            RuleFor(request => request.codBarras)
                .MustBeValidInternalCode()
                .When(request => !string.IsNullOrWhiteSpace(request.codBarras) && !request.codBarras.All(char.IsDigit))
                .WithMessage("Código interno deve ter entre 6 e 20 caracteres alfanuméricos");

            RuleFor(request => request.nomeProduto)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .Length(2, 100).WithMessage("O nome do produto deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\-\.]+$").WithMessage("O nome do produto contém caracteres inválidos.");

            RuleFor(request => request.descricaoProduto)
                .NotEmpty().WithMessage("A descrição do produto é obrigatória.")
                .Length(5, 500).WithMessage("A descrição deve ter entre 5 e 500 caracteres.");

            // Price, margin, dates and stock validations removed - these are now in separate domains

            RuleFor(request => request.FornecedorId)
                .NotEmpty().WithMessage("O ID do fornecedor é obrigatório.");

            RuleFor(request => request.CategoriaId)
                .NotEmpty().WithMessage("O ID da categoria é obrigatório.");

            RuleFor(request => request.statusAtivo)
                .InclusiveBetween(0, 1).WithMessage("O Status deve ser 0 (inativo) ou 1 (ativo).");

            // Business rule validations removed - prices and margins are now in separate domain
        }

        // Margin validation method removed - prices are now in separate domain
    }
}
