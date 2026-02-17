using FluentAssertions;
using Model;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests;

public class ModelTests
{
    [Fact]
    public void Cliente_Constructor_ShouldSetProperties()
    {
        // Arrange
        var CpfCnpj = "12345678901";
        var tipoCliente = "Físico";
        
        // Act
        var cliente = new Cliente(CpfCnpj, tipoCliente);
        
        // Assert
        cliente.CpfCnpj.Should().Be(CpfCnpj);
        cliente.TipoCliente.Should().Be(tipoCliente);
    }

    [Fact]
    public void Produto_IsLowStock_ShouldReturnTrue_WhenStockBelowReorderPoint()
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = 5;
        produto.ReorderPoint = 10;
        
        // Act
        var result = produto.IsLowStock();
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Produto_IsOutOfStock_ShouldReturnTrue_WhenStockIsZero()
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = 0;
        
        // Act
        var result = produto.IsOutOfStock();
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Produto_HasSufficientStock_ShouldReturnTrue_WhenStockIsEnough()
    {
        // Arrange
        var produto = new Produto();
        produto.QuantidadeEstoqueProduto = 100;
        var requestedQuantity = 50;
        
        // Act
        var result = produto.HasSufficientStock(requestedQuantity);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Usuario_Constructor_ShouldSetProperties()
    {
        // Arrange
        var login = "testuser";
        var senha = "password123";
        var StatusAtivo = true;
        var id = Guid.NewGuid();
        
        // Act
        var usuario = new Usuario(login, senha, StatusAtivo, id);
        
        // Assert
        usuario.Login.Should().Be(login);
        usuario.Senha.Should().Be(senha);
        usuario.StatusAtivo.Should().Be(StatusAtivo);
        usuario.Id.Should().Be(id);
    }
}