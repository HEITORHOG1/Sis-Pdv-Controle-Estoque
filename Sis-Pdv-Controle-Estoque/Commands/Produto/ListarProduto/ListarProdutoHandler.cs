using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Produto.ListarProduto
{
    public class ListarProdutoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorIdHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarProdutoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var _produto = _repositoryProduto.Listar().Include(x => x.Fornecedor).Include(x => x.Categoria).ToList();
            List<ListarProdutoRequest> _lista = new List<ListarProdutoRequest>();

            foreach (var item in _produto)
            {
                ListarProdutoRequest produto = new ListarProdutoRequest
                {
                    Id = item.Id,
                    codBarras = item.codBarras,
                    nomeProduto = item.nomeProduto,
                    NomeFornecedor = item.Fornecedor.nomeFantasia,
                    NomeCategoria = item.Categoria.NomeCategoria,
                    descricaoProduto = item.descricaoProduto,
                    quatidadeEstoqueProduto = item.quatidadeEstoqueProduto,
                    precoVenda = item.precoVenda,
                    precoCusto = item.precoCusto,
                    margemLucro = item.margemLucro,
                    dataFabricao = item.dataFabricao,
                    dataVencimento = item.dataVencimento,
                    statusAtivo = item.statusAtivo
                };
                _lista.Add(produto);
            }

            //var result = new { Id = _produto., NomeProduto = _produto.NomeProduto};

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, _lista);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

