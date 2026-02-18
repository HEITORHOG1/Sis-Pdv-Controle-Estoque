using FluentAssertions;
using Validators;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Validators;

public class CpfCnpjValidatorTests
{
    #region CPF Tests

    [Theory]
    [InlineData("52998224725")]    // Valid CPF without formatting
    [InlineData("529.982.247-25")] // Valid CPF with formatting
    public void IsValidCpf_ShouldReturnTrue_WhenCpfIsValid(string cpf)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCpf(cpf);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void IsValidCpf_ShouldReturnFalse_WhenCpfIsNullOrEmpty(string? cpf)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCpf(cpf!);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("12345678901")]   // Invalid check digits
    [InlineData("12345")]         // Too short
    [InlineData("9999999999")]    // Wrong length (10 digits)
    public void IsValidCpf_ShouldReturnFalse_WhenCpfIsInvalid(string cpf)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCpf(cpf);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region CNPJ Tests

    [Theory]
    [InlineData("11444777000161")]       // Valid CNPJ without formatting
    [InlineData("11.444.777/0001-61")]   // Valid CNPJ with formatting
    public void IsValidCnpj_ShouldReturnTrue_WhenCnpjIsValid(string cnpj)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCnpj(cnpj);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void IsValidCnpj_ShouldReturnFalse_WhenCnpjIsNullOrEmpty(string? cnpj)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCnpj(cnpj!);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("00000000000000")]  // All same digits
    [InlineData("11111111111111")]
    [InlineData("12345678000199")]  // Invalid check digits
    [InlineData("1234")]            // Too short
    public void IsValidCnpj_ShouldReturnFalse_WhenCnpjIsInvalid(string cnpj)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCnpj(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region IsValidCpfOrCnpj Tests

    [Theory]
    [InlineData("52998224725")]          // Valid CPF
    [InlineData("529.982.247-25")]       // Valid formatted CPF
    [InlineData("11444777000161")]       // Valid CNPJ
    [InlineData("11.444.777/0001-61")]   // Valid formatted CNPJ
    public void IsValidCpfOrCnpj_ShouldReturnTrue_WhenDocumentIsValid(string document)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCpfOrCnpj(document);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("1234567")]       // Invalid length (not 11 or 14)
    [InlineData("12345678901")]   // Invalid CPF (bad check digits)
    public void IsValidCpfOrCnpj_ShouldReturnFalse_WhenDocumentIsInvalid(string? document)
    {
        // Act
        var result = CpfCnpjValidator.IsValidCpfOrCnpj(document!);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
