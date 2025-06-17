using MediatR;

namespace Commands.Categoria.AlterarCategoria
{
    public class AlterarCategoriaRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string NomeCategoria { get; set; }
    }
}
