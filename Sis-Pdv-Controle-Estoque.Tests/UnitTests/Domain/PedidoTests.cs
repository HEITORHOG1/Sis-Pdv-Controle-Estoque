using FluentAssertions;
using Model;
using Model.Exceptions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class PedidoTests
{
    [Fact]
    public void Pedido_Constructor_ShouldSetProperties_WhenValid()
    {
        // Arrange & Act
        var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid(), 1, DateTime.Now, "Dinheiro", 100.50m);

        // Assert
        pedido.FormaPagamento.Should().Be("Dinheiro");
        pedido.TotalPedido.Should().Be(100.50m);
        pedido.Status.Should().Be(1);
    }

    [Fact]
    public void Pedido_Constructor_ShouldAcceptNullClienteId()
    {
        // Arrange & Act
        var pedido = new Pedido(Guid.NewGuid(), null, 0, DateTime.Now, "Cartão", 50m);

        // Assert
        pedido.ClienteId.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Pedido_Constructor_ShouldThrowDomainException_WhenFormaPagamentoEmpty(string formaPagamento)
    {
        // Act
        var act = () => new Pedido(Guid.NewGuid(), null, 0, DateTime.Now, formaPagamento, 50m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*forma de pagamento*");
    }

    [Fact]
    public void Pedido_Constructor_ShouldThrowDomainException_WhenTotalNegativo()
    {
        // Act
        var act = () => new Pedido(Guid.NewGuid(), null, 0, DateTime.Now, "Dinheiro", -10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*negativo*");
    }

    [Fact]
    public void Pedido_Constructor_ShouldAcceptZeroTotal()
    {
        // Act
        var pedido = new Pedido(Guid.NewGuid(), null, 0, DateTime.Now, "Dinheiro", 0m);

        // Assert
        pedido.TotalPedido.Should().Be(0m);
    }
}
