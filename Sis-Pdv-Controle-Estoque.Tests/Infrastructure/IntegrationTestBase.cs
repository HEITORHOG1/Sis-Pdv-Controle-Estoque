using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Base;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<TestFixture>, IDisposable
{
    protected readonly TestFixture Fixture;
    protected readonly IMediator Mediator;
    protected readonly IServiceScope Scope;

    protected IntegrationTestBase(TestFixture fixture)
    {
        Fixture = fixture;
        Scope = fixture.ServiceProvider.CreateScope();
        Mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    protected T GetService<T>() where T : notnull
    {
        return Scope.ServiceProvider.GetRequiredService<T>();
    }

    public virtual void Dispose()
    {
        Scope?.Dispose();
    }
}