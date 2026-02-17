using Commands.Cliente.AdicionarCliente;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Validators;

public class AdicionarClienteRequestValidatorTests
{
    private readonly AdicionarClienteRequestValidator _validator;

    public AdicionarClienteRequestValidatorTests()
    {
        _validator = new AdicionarClienteRequestValidator();
    }

    [Fact]
    public void Validator_ShouldPass_WhenValidCpfAndTipoFisico()
    {
        // Arrange - Using a valid CPF: 529.982.247-25
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "52998224725",
            TipoCliente = "Físico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_ShouldPass_WhenValidCnpjAndTipoJuridico()
    {
        // Arrange - Using a valid CNPJ: 11.444.777/0001-61
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "11444777000161",
            TipoCliente = "Jurídico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_ShouldFail_WhenCpfCnpjEmpty()
    {
        // Arrange
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "",
            TipoCliente = "Físico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.CpfCnpj);
    }

    [Fact]
    public void Validator_ShouldFail_WhenTipoClienteEmpty()
    {
        // Arrange - Using a valid CPF
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "52998224725",
            TipoCliente = ""
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.TipoCliente);
    }

    [Theory]
    [InlineData("52998224725", "Jurídico", false)] // Valid CPF with tipo Jurídico
    [InlineData("11444777000161", "Físico", false)] // Valid CNPJ with tipo Físico
    public void Validator_ShouldFail_WhenDocumentTypeMismatch(string documento, string tipoCliente, bool expectedResult)
    {
        // Arrange
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = documento,
            TipoCliente = tipoCliente
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("InvalidTipo")]
    [InlineData("Pessoa")]
    [InlineData("Outro")]
    public void Validator_ShouldFail_WhenTipoClienteInvalid(string tipoCliente)
    {
        // Arrange - Using a valid CPF
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "52998224725",
            TipoCliente = tipoCliente
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.TipoCliente);
    }

    [Theory]
    [InlineData("123")] // Too short
    [InlineData("123456789012345678901")] // Too long
    public void Validator_ShouldFail_WhenCpfCnpjInvalidLength(string documento)
    {
        // Arrange
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = documento,
            TipoCliente = "Físico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldAcceptFormattedCpf()
    {
        // Arrange - Using a valid formatted CPF: 529.982.247-25
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "529.982.247-25",
            TipoCliente = "Físico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        // The validator should handle formatted documents
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_ShouldAcceptFormattedCnpj()
    {
        // Arrange - Using a valid formatted CNPJ: 11.444.777/0001-61
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "11.444.777/0001-61",
            TipoCliente = "Jurídico"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
