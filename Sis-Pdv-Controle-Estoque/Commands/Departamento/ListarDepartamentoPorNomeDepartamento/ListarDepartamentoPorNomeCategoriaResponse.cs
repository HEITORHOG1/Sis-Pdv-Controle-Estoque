using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamentoPorNomeDepartamento
{
    public class ListarDepartamentoPorNomeDepartamentoResponse
    {
        public ListarDepartamentoPorNomeDepartamentoResponse(INotifiable notifiable)
        {
            this.Success = notifiable.IsValid();
            this.Notifications = notifiable.Notifications;
        }
        public ListarDepartamentoPorNomeDepartamentoResponse(INotifiable notifiable, object data)
        {
            this.Success = notifiable.IsValid();
            this.Data = data;
            this.Notifications = notifiable.Notifications;
        }

        public IEnumerable<Notification> Notifications { get; }
        public bool Success { get; private set; }
        public object Data { get; private set; }
    }
}
