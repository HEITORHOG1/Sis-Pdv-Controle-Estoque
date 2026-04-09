using Commands.Cliente.RemoverCliente;
using FluentAssertions;
using Interfaces;
using Model;
using Moq;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Handlers;

public class RemoverClienteHandlerTests
{
    private readonly Mock<IRepositoryCliente> _mockRepository;
    private readonly RemoverClienteHandler _handler;

    public RemoverClienteHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryCliente>();
        _handler = new RemoverClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldRemoveCliente()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente("52998224725", "Físico") { Id = clienteId };

        var request = new RemoverClienteRequest(clienteId);

        _mockRepository.Setup(r => r.ObterPorId(clienteId))
            .Returns(cliente);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        _mockRepository.Verify(r => r.Remover(It.IsAny<Cliente>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ClienteNaoEncontrado_ShouldReturnError()
    {
        // Arrange
        var request = new RemoverClienteRequest(Guid.NewGuid());

        _mockRepository.Setup(r => r.ObterPorId(request.Id))
            .Returns((Cliente?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        _mockRepository.Verify(r => r.Remover(It.IsAny<Cliente>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ClienteJaRemovido_ShouldReturnError()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente("52998224725", "Físico") { Id = clienteId, IsDeleted = true };

        var request = new RemoverClienteRequest(clienteId);

        _mockRepository.Setup(r => r.ObterPorId(clienteId))
            .Returns(cliente);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        _mockRepository.Verify(r => r.Remover(It.IsAny<Cliente>()), Times.Never);
    }
}
