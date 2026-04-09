namespace MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(BaseMessage message, string queueName);
    }
}
