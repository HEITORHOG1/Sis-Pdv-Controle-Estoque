using MediatR;

namespace Commands.Categoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaRequest : IRequest<ListarCategoriaPorNomeCategoriaResponse>
    {
        public ListarCategoriaPorNomeCategoriaRequest(string nomeCategoria)
        {
            NomeCategoria = nomeCategoria;
        }
        public string NomeCategoria { get; set; }
    }
}

