using MessageBus;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Sis_Pdv_Controle_Estoque.Model;
using RabbitMQ.Client.Exceptions;

namespace Sis_Pdv_Controle_Estoque_API.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        private readonly ILogger<RabbitMQMessageSender> _logger;

        public RabbitMQMessageSender(ILogger<RabbitMQMessageSender> logger)
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
            _logger = logger;
        }

        public RabbitMQMessageSender()
        {
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };
                _connection = factory.CreateConnection();

                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError("Não foi possível alcançar o broker do RabbitMQ. Verifique as configurações de conexão. Exceção: {0}", ex.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu uma exceção durante o envio da mensagem. Exceção: {0}", ex.ToString());
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
