using MediatR;

namespace Commands.Categoria.AdicionarCategoria
{
    public class AdicionarCategoriaRequest : IRequest<Response>
    {
        public string? NomeCategoria { get; set; }
    }
}
