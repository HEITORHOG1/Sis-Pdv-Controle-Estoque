using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdHandler : Notifiable, IRequestHandler<ListarColaboradorPorIdRequest, ListarColaboradorPorIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;

        public ListarColaboradorPorIdHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
        }

        public async Task<ListarColaboradorPorIdResponse> Handle(ListarColaboradorPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return null;
            }

            Sis_Pdv_Controle_Estoque.Model.Colaborador Collection = _repositoryColaborador.ObterPor(x=> x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarColaboradorPorIdResponse)Collection;

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

