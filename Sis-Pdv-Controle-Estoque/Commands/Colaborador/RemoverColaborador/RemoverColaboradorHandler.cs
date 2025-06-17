using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.RemoverColaborador

{
    public class RemoverColaboradorHandler : Notifiable, IRequestHandler<RemoverColaboradorResquest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryUsuario _repositoryUsuario;
        public RemoverColaboradorHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador, IRepositoryUsuario repositoryUsuario)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(RemoverColaboradorResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Colaborador Colaborador = _repositoryColaborador.Listar().
                Include(x => x.Usuario).
                Where(x => x.Id == request.Id).FirstOrDefault();

            if (Colaborador == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }
            if (Colaborador.Id == null)
            {
                AddNotification("Request", "Colaborador.Id");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }
            //if (Colaborador.Usuario.Id == null)
            //{
            //    AddNotification("Request", "Colaborador.Usuario.Id");
            //    return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            //}
            _repositoryUsuario.Remover(Colaborador.Usuario);
            _repositoryColaborador.Remover(Colaborador);

            var result = new { Colaborador.Id };

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, Colaborador);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
