using FluentValidation;
using Validators;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoRequestValidator : AbstractValidator<AdicionarProdutoRequest>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public AdicionarProdutoRequestValidator(IRepositoryFornecedor repositoryFornecedor, IRepositoryCategoria repositoryCategoria)
        {
            _repositoryFornecedor = repositoryFornecedor;
            _repositoryCategoria = repositoryCategoria;

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

            RuleFor(request => request.precoCusto)
                .MustBeValidPrice()
                .GreaterThan(0).WithMessage("O preço de custo deve ser maior que zero.");

            RuleFor(request => request.precoVenda)
                .MustBeValidPrice()
                .GreaterThan(0).WithMessage("O preço de venda deve ser maior que zero.")
                .GreaterThan(request => request.precoCusto).WithMessage("O preço de venda deve ser maior que o preço de custo.");

            RuleFor(request => request.margemLucro)
                .MustBeValidPercentage()
                .GreaterThan(0).WithMessage("A margem de lucro deve ser maior que zero.");

            RuleFor(request => request.dataFabricao)
                .NotEmpty().WithMessage("A data de fabricação é obrigatória.")
                .MustBeValidPastDate()
                .MustBeValidBusinessDate();

            RuleFor(request => request.dataVencimento)
                .NotEmpty().WithMessage("A data de vencimento é obrigatória.")
                .GreaterThan(request => request.dataFabricao).WithMessage("A data de vencimento deve ser após a data de fabricação.")
                .MustBeValidBusinessDate();

            RuleFor(request => request.quatidadeEstoqueProduto)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a zero.")
                .LessThan(1000000).WithMessage("Quantidade em estoque muito alta, verifique o valor.");

            RuleFor(request => request.FornecedorId)
                .NotEmpty().WithMessage("O ID do fornecedor é obrigatório.")
                .Must(FornecedorExiste).WithMessage("Fornecedor não encontrado. Verifique se o fornecedor selecionado ainda existe.");

            RuleFor(request => request.CategoriaId)
                .NotEmpty().WithMessage("O ID da categoria é obrigatório.")
                .Must(CategoriaExiste).WithMessage("Categoria não encontrada. Verifique se a categoria selecionada ainda existe.");

            RuleFor(request => request.statusAtivo)
                .InclusiveBetween(0, 1).WithMessage("O Status deve ser 0 (inativo) ou 1 (ativo).");

            // Business rule validation: margin calculation
            RuleFor(request => request)
                .Must(ValidarMargemLucro)
                .WithMessage("A margem de lucro não confere com a diferença entre preço de venda e custo.")
                .WithName("margemLucro");
        }

        private bool FornecedorExiste(Guid fornecedorId)
        {
            if (fornecedorId == Guid.Empty) return false;
            return _repositoryFornecedor.Existe(f => f.Id == fornecedorId && f.StatusAtivo == 1);
        }

        private bool CategoriaExiste(Guid categoriaId)
        {
            if (categoriaId == Guid.Empty) return false;
            return _repositoryCategoria.Existe(c => c.Id == categoriaId);
        }

        private bool ValidarMargemLucro(AdicionarProdutoRequest request)
        {
            if (request.precoCusto <= 0 || request.precoVenda <= 0)
                return true; // Let other validators handle invalid prices

            var margemCalculada = ((request.precoVenda - request.precoCusto) / request.precoCusto) * 100;
            var diferenca = Math.Abs(margemCalculada - request.margemLucro);
            
            return diferenca <= 0.01m; // Allow small rounding differences
        }
    }
}
