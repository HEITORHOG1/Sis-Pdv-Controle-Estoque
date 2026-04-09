using FluentAssertions;
using Model;
using Model.Exceptions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class DepartamentoTests
{
    [Fact]
    public void Departamento_Constructor_ShouldSetNomeDepartamento()
    {
        // Arrange & Act
        var departamento = new Departamento("Financeiro");

        // Assert
        departamento.NomeDepartamento.Should().Be("Financeiro");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Departamento_Constructor_ShouldThrowDomainException_WhenNomeEmpty(string nome)
    {
        // Act
        var act = () => new Departamento(nome);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*obrigatório*");
    }

    [Fact]
    public void Departamento_Constructor_ShouldThrowDomainException_WhenNomeTooShort()
    {
        // Act
        var act = () => new Departamento("A");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*mínimo 2*");
    }

    [Fact]
    public void Departamento_Constructor_ShouldThrowDomainException_WhenNomeTooLong()
    {
        // Act
        var act = () => new Departamento(new string('A', 101));

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*máximo 100*");
    }

    [Fact]
    public void Departamento_AlterarDepartamento_ShouldUpdateProperties()
    {
        // Arrange
        var departamento = new Departamento("Original");
        var novoId = Guid.NewGuid();

        // Act
        departamento.AlterarDepartamento(novoId, "Alterado");

        // Assert
        departamento.Id.Should().Be(novoId);
        departamento.NomeDepartamento.Should().Be("Alterado");
    }

    [Fact]
    public void Departamento_AlterarDepartamento_ShouldThrowDomainException_WhenNomeEmpty()
    {
        // Arrange
        var departamento = new Departamento("Original");

        // Act
        var act = () => departamento.AlterarDepartamento(Guid.NewGuid(), "");

        // Assert
        act.Should().Throw<DomainException>();
    }
}
