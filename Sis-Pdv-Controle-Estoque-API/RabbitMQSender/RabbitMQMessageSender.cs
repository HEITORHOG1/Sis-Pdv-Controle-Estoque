using MessageBus;
using Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_API.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string? _hostName;
        private readonly string? _password;
        private readonly string? _userName;
        private readonly ILogger<RabbitMQMessageSender>? _logger;

        public RabbitMQMessageSender(ILogger<RabbitMQMessageSender> logger)
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
            _logger = logger;
        }

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }
        public async void SendMessage(BaseMessage message, string queueName)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName ?? "localhost",
                    UserName = _userName ?? "guest",
                    Password = _password ?? "guest"
                };

                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                byte[] body = GetMessageAsByteArray(message);
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
            }
            catch (BrokerUnreachableException ex)
            {
                _logger?.LogError("Não foi possível alcançar o broker do RabbitMQ. Verifique as configurações de conexão. Exceção: {0}", ex.ToString());
            }
            catch (Exception ex)
            {
                _logger?.LogError("Ocorreu uma exceção durante o envio da mensagem. Exceção: {0}", ex.ToString());
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<CupomDTO>((CupomDTO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
