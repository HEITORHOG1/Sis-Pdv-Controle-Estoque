using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedorPorNomeFornecedor
{
    public class ListarFornecedorPorNomeFornecedorResponse
    {
        public ListarFornecedorPorNomeFornecedorResponse(INotifiable notifiable)
        {
            Success = notifiable.IsValid();
            Notifications = notifiable.Notifications;
        }
        public ListarFornecedorPorNomeFornecedorResponse(INotifiable notifiable, object data)
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
