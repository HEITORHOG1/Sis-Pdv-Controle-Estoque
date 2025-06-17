using prmToolkit.NotificationPattern;

namespace Commands
{
    public class Response
    {
        public Response(INotifiable notifiable)
        {
            Success = notifiable.IsValid();
            Notifications = notifiable.Notifications;
        }

        public Response(INotifiable notifiable, object data)
        {
            Success = notifiable.IsValid();
            Data = data;
            Notifications = notifiable.Notifications;
        }

        public IEnumerable<Notification> Notifications { get; }
        public bool Success { get; private set; }
        public object Data { get; private set; }

        //public void SetResult<TData>(TData data) where TData : class;
        //public void SetResult(object data);
    }
}
