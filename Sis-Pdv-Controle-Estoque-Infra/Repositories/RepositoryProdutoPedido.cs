using Commands.ProdutoPedido.ListarProdutoPedidoPorId;
using Dapper;
using MySql.Data.MySqlClient;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryProdutoPedido : RepositoryBase<ProdutoPedido, Guid>, IRepositoryProdutoPedido
    {
        private readonly PdvContext _context;
        public RepositoryProdutoPedido(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ListarProdutosPorPedidoIdResponse>> ListarProdutosPorPedidoId(Guid Id)
        {
            var sql = @"select 
                            pp.quantidadeItemPedido, 
                            p.nomeProduto, 
                            p.precoVenda,  
                            pp.totalProdutoPedido 
                            from 
                            ProdutoPedido pp 
                            join Produto p on pp.ProdutoId = p.Id 
                            join Pedido pe  on pe.Id = pp.PedidoId  " +
                            $"where pe.id = '{Id}';";

            using (var connection = new MySqlConnection("Server=localhost;Database=PDV_02;Uid=root;Pwd=q1w2e3r4;"))
            {
                connection.Open();
                return connection.Query(sql)
                            .Select(row => new ListarProdutosPorPedidoIdResponse
                            {
                                quantidadeItemPedido = (int)row.quantidadeItemPedido,
                                nomeProduto = (string)row.nomeProduto,
                                precoVenda = (decimal)row.precoVenda,
                                totalProdutoPedido = (decimal)row.totalProdutoPedido
                            }
                            ).ToList();
            }
        }
    }
}
