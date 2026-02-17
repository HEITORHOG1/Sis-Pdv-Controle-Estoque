using FluentAssertions;
using Model;
using Model.Exceptions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class ProdutoTests
{
    private readonly Guid _fornecedorId = Guid.NewGuid();
    private readonly Guid _categoriaId = Guid.NewGuid();

    [Fact]
    public void Produto_Constructor_ShouldSetProperties_WhenValidParameters()
    {
        // Arrange
        var codBarras = "7891234567890";
        var nomeProduto = "Produto Teste";
        var descricaoProduto = "Descrição do produto teste";
        var precoCusto = 10.0m;
        var precoVenda = 15.0m;
        var margemLucro = 50.0m;
        var dataFabricacao = DateTime.Now.AddDays(-30);
        var dataVencimento = DateTime.Now.AddDays(365);
        var quantidadeEstoque = 100;

        // Act
        var produto = new Produto(
            codBarras,
            nomeProduto,
            descricaoProduto,
            precoCusto,
            precoVenda,
            margemLucro,
            dataFabricacao,
            dataVencimento,
            quantidadeEstoque,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Assert
        produto.CodBarras.Should().Be(codBarras);
        produto.NomeProduto.Should().Be(nomeProduto);
        produto.DescricaoProduto.Should().Be(descricaoProduto);
        produto.PrecoCusto.Should().Be(precoCusto);
        produto.PrecoVenda.Should().Be(precoVenda);
        produto.MargemLucro.Should().Be(margemLucro);
        produto.DataFabricao.Should().Be(dataFabricacao);
        produto.DataVencimento.Should().Be(dataVencimento);
        produto.QuantidadeEstoqueProduto.Should().Be(quantidadeEstoque);
        produto.FornecedorId.Should().Be(_fornecedorId);
        produto.CategoriaId.Should().Be(_categoriaId);
        produto.StatusAtivo.Should().Be(1);
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenCodigoBarrasEmpty()
    {
        // Arrange
        Action act = () => new Produto(
            "",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Código de barras é obrigatório");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenNomeProdutoEmpty()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome do produto é obrigatório");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenNomeProdutoTooShort()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "AB",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome do produto deve ter no mínimo 3 caracteres");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenPrecoVendaLessThanPrecoCusto()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            20.0m,
            10.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Preço de venda não pode ser menor que o preço de custo");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenPrecoVendaZero()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Preço de venda deve ser maior que zero");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenDataVencimentoBeforeDataFabricacao()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(30),
            DateTime.Now.AddDays(-30),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Data de vencimento deve ser posterior à data de fabricação");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenQuantidadeEstoqueNegativa()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            -10,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Quantidade em estoque não pode ser negativa");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenFornecedorIdEmpty()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            Guid.Empty,
            _categoriaId,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Fornecedor é obrigatório");
    }

    [Fact]
    public void Produto_Constructor_ShouldThrowDomainException_WhenCategoriaIdEmpty()
    {
        // Arrange
        Action act = () => new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            Guid.Empty,
            1
        );

        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria é obrigatória");
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(10, 10, true)]
    [InlineData(15, 10, false)]
    public void Produto_IsLowStock_ShouldReturnCorrectResult(int quantidade, int reorderPoint, bool expectedResult)
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = quantidade;
        produto.ReorderPoint = reorderPoint;

        // Act
        var result = produto.IsLowStock();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(-5, true)]
    [InlineData(10, false)]
    public void Produto_IsOutOfStock_ShouldReturnCorrectResult(int quantidade, bool expectedResult)
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = quantidade;

        // Act
        var result = produto.IsOutOfStock();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(100, 50, true)]
    [InlineData(50, 50, true)]
    [InlineData(30, 50, false)]
    public void Produto_HasSufficientStock_ShouldReturnCorrectResult(int quantidadeEstoque, int quantidadeSolicitada, bool expectedResult)
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = quantidadeEstoque;

        // Act
        var result = produto.HasSufficientStock(quantidadeSolicitada);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Produto_UpdateStock_ShouldUpdateQuantityAndCreateMovement()
    {
        // Arrange
        var produto = new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act
        produto.UpdateStock(150, "Entrada de estoque", Guid.NewGuid());

        // Assert
        produto.QuantidadeEstoqueProduto.Should().Be(150);
        produto.StockMovements.Should().HaveCount(1);
        produto.StockMovements.First().Quantity.Should().Be(50);
        produto.StockMovements.First().Type.Should().Be(StockMovementType.Entry);
    }

    [Fact]
    public void Produto_UpdateStock_ShouldThrowDomainException_WhenQuantityNegative()
    {
        // Arrange
        var produto = new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act
        Action act = () => produto.UpdateStock(-10, "Teste");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("A quantidade em estoque não pode ser negativa");
    }

    [Fact]
    public void Produto_UpdateStock_ShouldThrowDomainException_WhenReasonEmpty()
    {
        // Arrange
        var produto = new Produto(
            "7891234567890",
            "Produto Teste",
            "Descrição",
            10.0m,
            15.0m,
            50.0m,
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(365),
            100,
            _fornecedorId,
            _categoriaId,
            1
        );

        // Act
        Action act = () => produto.UpdateStock(150, "");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Motivo da movimentação de estoque é obrigatório");
    }
}
