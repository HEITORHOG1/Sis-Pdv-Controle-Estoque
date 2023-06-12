using MessageBus;

namespace Sis_Pdv_Controle_Estoque_API.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
