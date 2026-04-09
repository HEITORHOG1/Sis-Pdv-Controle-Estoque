using FluentAssertions;
using Model;
using Model.Exceptions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class ProdutoPedidoTests
{
    [Fact]
    public void ProdutoPedido_Constructor_ShouldSetProperties_WhenValid()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();

        // Act
        var item = new ProdutoPedido(pedidoId, produtoId, "7891234567890", 3, 59.90m);

        // Assert
        item.PedidoId.Should().Be(pedidoId);
        item.ProdutoId.Should().Be(produtoId);
        item.QuantidadeItemPedido.Should().Be(3);
        item.TotalProdutoPedido.Should().Be(59.90m);
    }

    [Fact]
    public void ProdutoPedido_Constructor_ShouldThrowDomainException_WhenPedidoIdEmpty()
    {
        // Act
        var act = () => new ProdutoPedido(Guid.Empty, Guid.NewGuid(), "123", 1, 10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*pedido*");
    }

    [Fact]
    public void ProdutoPedido_Constructor_ShouldThrowDomainException_WhenProdutoIdEmpty()
    {
        // Act
        var act = () => new ProdutoPedido(Guid.NewGuid(), Guid.Empty, "123", 1, 10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*produto*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ProdutoPedido_Constructor_ShouldThrowDomainException_WhenQuantidadeInvalida(int quantidade)
    {
        // Act
        var act = () => new ProdutoPedido(Guid.NewGuid(), Guid.NewGuid(), "123", quantidade, 10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*quantidade*");
    }

    [Fact]
    public void ProdutoPedido_Constructor_ShouldSetCodBarras()
    {
        // Arrange & Act
        var item = new ProdutoPedido(Guid.NewGuid(), Guid.NewGuid(), "7891234567890", 2, 39.90m);

        // Assert
        item.CodBarras.Should().Be("7891234567890");
    }
}
