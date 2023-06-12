using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public decimal precoCusto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal margemLucro { get; set; }
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
        public Sis_Pdv_Controle_Estoque.Model.Fornecedor Fornecedor { get; set; }
        public Sis_Pdv_Controle_Estoque.Model.Categoria Categoria { get; set; }
        public int statusAtivo { get; set; }

        public static explicit operator ListarProdutoPorIdResponse(Sis_Pdv_Controle_Estoque.Model.Produto request)
        {
            return new ListarProdutoPorIdResponse()
            {
                codBarras = request.codBarras,
                nomeProduto = request.nomeProduto,
                descricaoProduto = request.descricaoProduto,
                precoCusto = request.precoCusto,
                precoVenda = request.precoVenda,
                margemLucro = request.margemLucro,
                dataFabricao = request.dataFabricao,
                dataVencimento = request.dataVencimento,
                quatidadeEstoqueProduto = request.quatidadeEstoqueProduto,
                Fornecedor = new Sis_Pdv_Controle_Estoque.Model.Fornecedor { Id = request.Id },
                Categoria =  new Sis_Pdv_Controle_Estoque.Model.Categoria { Id = request.Id } ,
                statusAtivo = request.statusAtivo
            };
        }
    }
}
