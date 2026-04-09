using Commands.Cliente.AlterarCliente;
using FluentAssertions;
using Interfaces;
using Model;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Handlers;

public class AlterarClienteHandlerTests
{
    private readonly Mock<IRepositoryCliente> _mockRepository;
    private readonly AlterarClienteHandler _handler;

    public AlterarClienteHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryCliente>();
        _handler = new AlterarClienteHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateCliente()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var clienteExistente = new Cliente("52998224725", "Físico") { Id = clienteId };

        var request = new AlterarClienteRequest
        {
            Id = clienteId,
            CpfCnpj = "11444777000161",
            TipoCliente = "Jurídico"
        };

        _mockRepository.Setup(r => r.ObterPorIdAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteExistente);

        _mockRepository.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.EditarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente c, CancellationToken _) => c);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        _mockRepository.Verify(r => r.EditarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ClienteNaoEncontrado_ShouldReturnError()
    {
        // Arrange
        var request = new AlterarClienteRequest
        {
            Id = Guid.NewGuid(),
            CpfCnpj = "52998224725",
            TipoCliente = "Físico"
        };

        _mockRepository.Setup(r => r.ObterPorIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        _mockRepository.Verify(r => r.EditarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateCpfCnpj_ShouldReturnError()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var clienteExistente = new Cliente("52998224725", "Físico") { Id = clienteId };

        var request = new AlterarClienteRequest
        {
            Id = clienteId,
            CpfCnpj = "11444777000161",
            TipoCliente = "Jurídico"
        };

        _mockRepository.Setup(r => r.ObterPorIdAsync(clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteExistente);

        _mockRepository.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}
