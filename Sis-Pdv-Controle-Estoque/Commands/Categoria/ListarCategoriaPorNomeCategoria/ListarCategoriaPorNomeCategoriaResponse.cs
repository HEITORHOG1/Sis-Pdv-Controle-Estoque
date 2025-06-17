using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaResponse
    {
        public ListarCategoriaPorNomeCategoriaResponse(INotifiable notifiable)
        {
            Success = notifiable.IsValid();
            Notifications = notifiable.Notifications;
        }
        public ListarCategoriaPorNomeCategoriaResponse(INotifiable notifiable, object data)
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
