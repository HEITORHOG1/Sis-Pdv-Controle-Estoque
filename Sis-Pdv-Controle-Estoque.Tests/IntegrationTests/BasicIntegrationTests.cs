using FluentAssertions;
using Sis_Pdv_Controle_Estoque.Tests.Infrastructure;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.IntegrationTests;

public class BasicIntegrationTests : IntegrationTestBase
{
    public BasicIntegrationTests(TestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void TestFixture_ShouldBeAccessible()
    {
        // Act & Assert
        Fixture.Should().NotBeNull();
        Fixture.ServiceProvider.Should().NotBeNull();
    }

    [Fact]
    public void Mediator_ShouldBeConfigured()
    {
        // Act & Assert
        Mediator.Should().NotBeNull();
    }

    [Fact]
    public void ServiceProvider_ShouldResolveServices()
    {
        // Act
        var service = GetService<Microsoft.Extensions.Logging.ILogger<BasicIntegrationTests>>();
        
        // Assert
        service.Should().NotBeNull();
    }
}