using FluentAssertions;
using Model;
using Model.Base;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class CategoriaTests
{
    [Fact]
    public void Categoria_Constructor_ShouldSetNomeCategoria()
    {
        // Arrange
        var nomeCategoria = "Eletrônicos";

        // Act
        var categoria = new Categoria(nomeCategoria);

        // Assert
        categoria.NomeCategoria.Should().Be(nomeCategoria);
    }

    [Fact]
    public void Categoria_Constructor_WithId_ShouldSetIdAndNomeCategoria()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nomeCategoria = "Eletrônicos";

        // Act
        var categoria = new Categoria(id, nomeCategoria);

        // Assert
        categoria.Id.Should().Be(id);
        categoria.NomeCategoria.Should().Be(nomeCategoria);
    }

    [Fact]
    public void Categoria_AlterarCategoria_ShouldUpdateProperties()
    {
        // Arrange
        var categoria = new Categoria("Categoria Original");
        var novoId = Guid.NewGuid();
        var novoNome = "Categoria Alterada";

        // Act
        categoria.AlterarCategoria(novoId, novoNome);

        // Assert
        categoria.Id.Should().Be(novoId);
        categoria.NomeCategoria.Should().Be(novoNome);
    }

    [Fact]
    public void Categoria_ShouldInheritFromEntityBase()
    {
        // Arrange
        var categoria = new Categoria();

        // Assert
        categoria.Should().BeAssignableTo<EntityBase>();
    }

    [Fact]
    public void Categoria_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var categoria = new Categoria();

        // Assert
        categoria.NomeCategoria.Should().Be(string.Empty);
        // EntityBase constructor sets Id = Guid.NewGuid(), so Id should not be empty
        categoria.Id.Should().NotBe(Guid.Empty);
        categoria.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
