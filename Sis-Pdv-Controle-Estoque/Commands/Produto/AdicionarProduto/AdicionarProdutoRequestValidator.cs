using FluentValidation;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoRequestValidator : AbstractValidator<AdicionarProdutoRequest>
    {
        public AdicionarProdutoRequestValidator()
        {
            RuleFor(request => request.codBarras).NotEmpty().WithMessage("O código de barras é obrigatório.");

            RuleFor(request => request.nomeProduto).NotEmpty().WithMessage("O nome do produto é obrigatório.");

            RuleFor(request => request.descricaoProduto).NotEmpty().WithMessage("A descrição do produto é obrigatória.");

            RuleFor(request => request.precoCusto).GreaterThan(0).WithMessage("O preço de custo deve ser maior que zero.");

            RuleFor(request => request.precoVenda).GreaterThan(0).WithMessage("O preço de venda deve ser maior que zero.");

            RuleFor(request => request.margemLucro).GreaterThan(0).WithMessage("A margem de lucro deve ser maior que zero.");

            RuleFor(request => request.dataFabricao).NotEmpty().WithMessage("A data de fabricação é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de fabricação não pode ser no futuro.");

            RuleFor(request => request.dataVencimento).NotEmpty().WithMessage("A data de vencimento é obrigatória.")
                .GreaterThan(request => request.dataFabricao).WithMessage("A data de vencimento deve ser após a data de fabricação.");

            RuleFor(request => request.quatidadeEstoqueProduto).GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a zero.");

            RuleFor(request => request.FornecedorId).NotEmpty().WithMessage("O ID do fornecedor é obrigatório.");

            RuleFor(request => request.CategoriaId).NotEmpty().WithMessage("O ID da categoria é obrigatório.");

            RuleFor(request => request.statusAtivo).InclusiveBetween(0, 1).WithMessage("O Status deve ser 0 (inativo) ou 1 (ativo).");
        }
    }
}
