using MediatR;

namespace Commands.Usuarios.ListarUsuarios
{
    public class ListarUsuariosRequest : IRequest<Response>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public bool? StatusAtivo { get; set; }
    }
}