using prmToolkit.NotificationPattern;

namespace Commands
{
    public class Response
    {
        // Parameterless constructor required for JSON deserialization in logging/middleware
        public Response()
        {
        }

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

        // Make settable to support deserialization scenarios
        public IEnumerable<Notification> Notifications { get; set; } = Enumerable.Empty<Notification>();
        public bool Success { get; set; }
        public object Data { get; set; }

        //public void SetResult<TData>(TData data) where TData : class;
        //public void SetResult(object data);
    }
}
