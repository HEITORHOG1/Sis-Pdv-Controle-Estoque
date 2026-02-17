using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaboradorPorNomeColaborador
{
    public class ListarColaboradorPorNomeColaboradorResponse
    {
        public ListarColaboradorPorNomeColaboradorResponse(INotifiable notifiable)
        {
            Success = notifiable.IsValid();
            Notifications = notifiable.Notifications;
        }
        public ListarColaboradorPorNomeColaboradorResponse(INotifiable notifiable, object data)
        {
            Success = notifiable.IsValid();
            Data = data;
            Notifications = notifiable.Notifications;
        }

        public IEnumerable<Notification> Notifications { get; }
        public bool Success { get; private set; }
        public object Data { get; private set; }
    }
}
