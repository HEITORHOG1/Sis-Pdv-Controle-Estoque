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
        var cpfCnpj = "12345678901";
        var tipoCliente = "Físico";
        
        // Act
        var cliente = new Cliente(cpfCnpj, tipoCliente);
        
        // Assert
        cliente.CpfCnpj.Should().Be(cpfCnpj);
        cliente.TipoCliente.Should().Be(tipoCliente);
    }

    [Fact]
    public void InventoryBalance_IsLowStock_ShouldReturnTrue_WhenStockBelowReorderPoint()
    {
        // Arrange
        var inv = new InventoryBalance(Guid.NewGuid(), currentStock: 5, reorderPoint: 10);
        
        // Act
        var result = inv.IsLowStock();
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InventoryBalance_IsOutOfStock_ShouldReturnTrue_WhenStockIsZero()
    {
        // Arrange
        var inv = new InventoryBalance(Guid.NewGuid(), currentStock: 0);
        
        // Act
        var result = inv.IsOutOfStock();
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InventoryBalance_HasSufficientStock_ShouldReturnTrue_WhenStockIsEnough()
    {
        // Arrange
        var inv = new InventoryBalance(Guid.NewGuid(), currentStock: 100);
        var requestedQuantity = 50;
        
        // Act
        var result = inv.HasSufficientStock(requestedQuantity);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Usuario_Constructor_ShouldSetProperties()
    {
        // Arrange
        var login = "testuser";
        var senha = "password123";
        var statusAtivo = true;
        var id = Guid.NewGuid();
        
        // Act
        var usuario = new Usuario(login, senha, statusAtivo, id);
        
        // Assert
        usuario.Login.Should().Be(login);
        usuario.Senha.Should().Be(senha);
        usuario.StatusAtivo.Should().Be(statusAtivo);
        usuario.Id.Should().Be(id);
    }
}