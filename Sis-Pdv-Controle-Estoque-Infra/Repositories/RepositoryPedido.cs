using Commands.Pedido.ListarVendaPedidoPorData;
using Dapper;
using MySql.Data.MySqlClient;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Base;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryPedido : RepositoryBase<Pedido, Guid>, IRepositoryPedido
    {
        private readonly PdvContext _context;
        public RepositoryPedido(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ListarVendaPedidoPorDataResponse>> ListarVendaPedidoPorData(DateTime DataInicio, DateTime DataFim)
        {
            var sql = @"SELECT id,
                            dataDoPedido,
                            formaPagamento,
                            totalPedido
                                    FROM pedido
                                    WHERE dataDoPedido " +
                                      $" BETWEEN '{DataInicio.Date.ToString("yyyy-MM-dd")}' " +
                                      $" AND     '{DataFim.Date.ToString("yyyy-MM-dd")}'";

            using (var connection = new MySqlConnection("Server=localhost;Database=PDV_02;Uid=root;Pwd=q1w2e3r4;"))
            {
                connection.Open();
                return  connection.Query(sql)
                            .Select(row => new ListarVendaPedidoPorDataResponse
                            {
                                Id = (Guid?) row.id,
                                dataDoPedido = (DateTime?) row.dataDoPedido,
                                formaPagamento = (string) row.formaPagamento,
                                totalPedido = (decimal) row.totalPedido
                            }
                            ).ToList();
            }
        }
    }
}
