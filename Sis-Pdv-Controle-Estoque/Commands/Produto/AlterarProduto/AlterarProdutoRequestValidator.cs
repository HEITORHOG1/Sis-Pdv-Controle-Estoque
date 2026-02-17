using FluentValidation;
using Validators;

namespace Commands.Produto.AlterarProduto
{
    public class AlterarProdutoRequestValidator : AbstractValidator<AlterarProdutoRequest>
    {
        public AlterarProdutoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("O ID do produto � obrigat�rio.");

            RuleFor(request => request.CodBarras)
                .NotEmpty().WithMessage("O c�digo de barras � obrigat�rio.")
                .MustBeValidBarcode()
                .When(request => !string.IsNullOrWhiteSpace(request.CodBarras) && request.CodBarras.All(char.IsDigit))
                .WithMessage("C�digo de barras inv�lido para formato EAN/UPC");

            RuleFor(request => request.CodBarras)
                .MustBeValidInternalCode()
                .When(request => !string.IsNullOrWhiteSpace(request.CodBarras) && !request.CodBarras.All(char.IsDigit))
                .WithMessage("C�digo interno deve ter entre 6 e 20 caracteres alfanum�ricos");

            RuleFor(request => request.NomeProduto)
                .NotEmpty().WithMessage("O nome do produto � obrigat�rio.")
                .Length(2, 100).WithMessage("O nome do produto deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-Z�-�0-9\s\-\.]+$").WithMessage("O nome do produto cont�m caracteres inv�lidos.");

            RuleFor(request => request.DescricaoProduto)
                .NotEmpty().WithMessage("A descri��o do produto � obrigat�ria.")
                .Length(5, 500).WithMessage("A descri��o deve ter entre 5 e 500 caracteres.");

            RuleFor(request => request.PrecoCusto)
                .MustBeValidPrice()
                .GreaterThan(0).WithMessage("O pre�o de custo deve ser maior que zero.");

            RuleFor(request => request.PrecoVenda)
                .MustBeValidPrice()
                .GreaterThan(0).WithMessage("O pre�o de venda deve ser maior que zero.")
                .GreaterThan(request => request.PrecoCusto).WithMessage("O pre�o de venda deve ser maior que o pre�o de custo.");

            RuleFor(request => request.MargemLucro)
                .MustBeValidPercentage()
                .GreaterThan(0).WithMessage("A margem de lucro deve ser maior que zero.");

            RuleFor(request => request.DataFabricao)
                .NotEmpty().WithMessage("A data de fabrica��o � obrigat�ria.")
                .MustBeValidPastDate()
                .MustBeValidBusinessDate();

            RuleFor(request => request.DataVencimento)
                .NotEmpty().WithMessage("A data de vencimento � obrigat�ria.")
                .GreaterThan(request => request.DataFabricao).WithMessage("A data de vencimento deve ser ap�s a data de fabrica��o.")
                .MustBeValidBusinessDate();

            RuleFor(request => request.QuantidadeEstoqueProduto)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a zero.")
                .LessThan(1000000).WithMessage("Quantidade em estoque muito alta, verifique o valor.");

            RuleFor(request => request.FornecedorId)
                .NotEmpty().WithMessage("O ID do fornecedor � obrigat�rio.");

            RuleFor(request => request.CategoriaId)
                .NotEmpty().WithMessage("O ID da categoria � obrigat�rio.");

            RuleFor(request => request.StatusAtivo)
                .InclusiveBetween(0, 1).WithMessage("O Status deve ser 0 (inativo) ou 1 (ativo).");

            // Business rule validation: margin calculation
            RuleFor(request => request)
                .Must(ValidarMargemLucro)
                .WithMessage("A margem de lucro n�o confere com a diferen�a entre pre�o de venda e custo.")
                .WithName("MargemLucro");
        }

        private bool ValidarMargemLucro(AlterarProdutoRequest request)
        {
            if (request.PrecoCusto <= 0 || request.PrecoVenda <= 0)
                return true; // Let other validators handle invalid prices

            var margemCalculada = ((request.PrecoVenda - request.PrecoCusto) / request.PrecoCusto) * 100;
            var diferenca = Math.Abs(margemCalculada - request.MargemLucro);
            
            return diferenca <= 0.01m; // Allow small rounding differences
        }
    }
}
