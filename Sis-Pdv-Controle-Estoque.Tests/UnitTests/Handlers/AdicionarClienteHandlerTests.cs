using Commands.Cliente.AdicionarCliente;
using FluentAssertions;
using Interfaces;
using Model;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Handlers;

public class AdicionarClienteHandlerTests
{
    private readonly Mock<IRepositoryCliente> _mockRepository;
    private readonly AdicionarClienteHandler _handler;

    public AdicionarClienteHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryCliente>();
        _handler = new AdicionarClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateCliente()
    {
        // Arrange
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "52998224725",
            TipoCliente = "Físico"
        };

        _mockRepository.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente c, CancellationToken _) => c);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateCpfCnpj_ShouldReturnError()
    {
        // Arrange
        var request = new AdicionarClienteRequest
        {
            CpfCnpj = "52998224725",
            TipoCliente = "Físico"
        };

        _mockRepository.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
