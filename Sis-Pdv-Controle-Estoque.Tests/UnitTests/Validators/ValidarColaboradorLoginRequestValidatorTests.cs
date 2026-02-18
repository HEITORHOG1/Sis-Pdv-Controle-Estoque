using Commands.Colaborador.ValidarColaboradorLogin;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Validators;

public class ValidarColaboradorLoginRequestValidatorTests
{
    private readonly ValidarColaboradorLoginRequestValidator _validator;

    public ValidarColaboradorLoginRequestValidatorTests()
    {
        _validator = new ValidarColaboradorLoginRequestValidator();
    }

    [Fact]
    public void Validator_ShouldPass_WhenValidLoginAndSenha()
    {
        // Arrange
        var request = new ValidarColaboradorLoginRequest("admin", "senha123");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "senha123")]
    [InlineData(null, "senha123")]
    public void Validator_ShouldFail_WhenLoginEmpty(string? login, string senha)
    {
        // Arrange
        var request = new ValidarColaboradorLoginRequest(login!, senha);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData("admin", "")]
    [InlineData("admin", null)]
    public void Validator_ShouldFail_WhenSenhaEmpty(string login, string? senha)
    {
        // Arrange
        var request = new ValidarColaboradorLoginRequest(login, senha!);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Senha);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("abc")]
    [InlineData("a")]
    public void Validator_ShouldFail_WhenSenhaTooShort(string senha)
    {
        // Arrange
        var request = new ValidarColaboradorLoginRequest("admin", senha);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Senha);
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("abcdef")]
    [InlineData("senha_segura_123")]
    public void Validator_ShouldPass_WhenSenhaHasMinimumLength(string senha)
    {
        // Arrange
        var request = new ValidarColaboradorLoginRequest("admin", senha);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
