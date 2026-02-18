using Commands.Usuarios.Login;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator;

    public LoginRequestValidatorTests()
    {
        _validator = new LoginRequestValidator();
    }

    [Fact]
    public void Validator_ShouldPass_WhenValidRequest()
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = "user@email.com",
            Password = "senha123"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validator_ShouldFail_WhenLoginEmpty(string? login)
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = login!,
            Password = "senha123"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    public void Validator_ShouldFail_WhenLoginExceedsMaxLength()
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = new string('a', 101),
            Password = "senha123"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validator_ShouldFail_WhenPasswordEmpty(string? password)
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = "admin",
            Password = password!
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("abc")]
    [InlineData("a")]
    public void Validator_ShouldFail_WhenPasswordTooShort(string password)
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = "admin",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validator_ShouldPass_WhenOptionalFieldsAreNull()
    {
        // Arrange
        var request = new LoginRequest
        {
            Login = "admin",
            Password = "senha123",
            IpAddress = null,
            UserAgent = null
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
