using System.Threading;
using System.Threading.Tasks;
using Commands.Categoria.AdicionarCliente;
using MediatR;
using Moq;
using Sis_Pdv_Controle_Estoque.Commands.Cliente;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests
{
    public class AdicionarClienteHandlerTests
    {
        [Fact]
        public async Task DuplicateCpfCnpj_ShouldAddNotification()
        {
            // Arrange
            var mediator = new Mock<IMediator>();
            var repository = new Mock<IRepositoryCliente>();
            repository.Setup(r => r.Existe(It.IsAny<Func<Sis_Pdv_Controle_Estoque.Model.Cliente, bool>>()))
                      .Returns(true);

            var handler = new AdicionarClienteHandler(mediator.Object, repository.Object);

            var request = new AdicionarClienteRequest
            {
                CpfCnpj = "12345678901",
                TipoCliente = "F\u00edsico"
            };

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            var notification = Assert.Single(response.Notifications);
            Assert.Equal("NomeCategoria", notification.Property);
            Assert.Equal("Categoria ja Cadastrada", notification.Message);
        }
    }
}
