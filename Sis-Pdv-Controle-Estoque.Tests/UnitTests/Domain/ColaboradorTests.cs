using FluentAssertions;
using Model;
using Model.Exceptions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class ColaboradorTests
{
    private static Usuario CriarUsuario() => new Usuario();

    [Fact]
    public void Colaborador_Constructor_ShouldSetProperties_WhenValid()
    {
        // Arrange & Act
        var id = Guid.NewGuid();
        var colaborador = new Colaborador(id, "João Silva", Guid.NewGuid(), "12345678901",
            "Desenvolvedor", "11999999999", "joao@pessoal.com", "joao@empresa.com", CriarUsuario());

        // Assert
        colaborador.NomeColaborador.Should().Be("João Silva");
        colaborador.CpfColaborador.Should().Be("12345678901");
        colaborador.Id.Should().Be(id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Colaborador_Constructor_ShouldThrowDomainException_WhenNomeEmpty(string nome)
    {
        // Act
        var act = () => new Colaborador(Guid.NewGuid(), nome, null, "12345678901",
            "Cargo", "11999999999", "email@test.com", "corp@test.com", CriarUsuario());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*nome*colaborador*");
    }

    [Fact]
    public void Colaborador_Constructor_ShouldThrowDomainException_WhenNomeTooShort()
    {
        // Act
        var act = () => new Colaborador(Guid.NewGuid(), "A", null, "12345678901",
            "Cargo", "11999999999", "email@test.com", "corp@test.com", CriarUsuario());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*mínimo 2*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Colaborador_Constructor_ShouldThrowDomainException_WhenCpfEmpty(string cpf)
    {
        // Act
        var act = () => new Colaborador(Guid.NewGuid(), "João Silva", null, cpf,
            "Cargo", "11999999999", "email@test.com", "corp@test.com", CriarUsuario());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*CPF*");
    }
}
