using MediatR;

namespace Commands.Categoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaRequest : IRequest<ListarCategoriaPorNomeCategoriaResponse>
    {
        public ListarCategoriaPorNomeCategoriaRequest(string NomeCategoria)
        {
            NomeCategoria = NomeCategoria;
        }
        public string NomeCategoria { get; set; }
    }
}

