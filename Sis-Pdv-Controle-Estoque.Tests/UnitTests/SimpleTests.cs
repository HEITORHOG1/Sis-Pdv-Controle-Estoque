using FluentAssertions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests;

public class SimpleTests
{
    [Fact]
    public void BasicTest_ShouldPass()
    {
        // Arrange
        var expected = "Hello World";
        
        // Act
        var actual = "Hello World";
        
        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    [InlineData(-1, 1, 0)]
    public void Addition_ShouldReturnCorrectSum(int a, int b, int expected)
    {
        // Act
        var result = a + b;
        
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void TestInfrastructure_ShouldWork()
    {
        // This test verifies that the test infrastructure is working
        Assert.True(true);
    }
}