using MediatR;
using Model;

namespace Commands.Produto.ListarProdutosPaginado
{
    public class ListarProdutosPaginadoRequest : PagedRequest, IRequest<Response>
    {
        public string? Categoria { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public bool? ApenasAtivos { get; set; } = true;
    }
}