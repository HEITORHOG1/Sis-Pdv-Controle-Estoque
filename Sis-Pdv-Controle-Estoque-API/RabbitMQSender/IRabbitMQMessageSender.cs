using MessageBus;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_API.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        Task SendMessageAsync(BaseMessage baseMessage, string queueName);
    }
}
