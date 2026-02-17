using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorHandler : Notifiable, IRequestHandler<AlterarColaboradorRequest, Response>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryUsuario _repositoryUsuario;
        public AlterarColaboradorHandler(IRepositoryColaborador repositoryColaborador, IRepositoryUsuario repositoryUsuario)
        {
            _repositoryColaborador = repositoryColaborador;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(AlterarColaboradorRequest request, CancellationToken cancellationToken)
        {

            Model.Colaborador Colaborador = new Model.Colaborador();

            var retornoExist = _repositoryColaborador.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Colaborador", "Colaborador nï¿½o existe");
                return new Response(this);
            }
            if (string.IsNullOrEmpty(request.Usuario.Id.ToString()))
            {
                AddNotification("Colaborador", "Colaborador nï¿½o existe");
                return new Response(this);
            }

            var _usuario = new Model.Colaborador
            {
                Usuario = new Model.Usuario
                {
                    Id = request.Usuario.Id,
                    Login = request.Usuario.Login,
                    Senha = request.Usuario.Senha,
                    StatusAtivo = request.Usuario.StatusAtivo
                }
            };
            Colaborador.AlterarColaborador(request.Id,
                                            request.NomeColaborador,
                                            request.DepartamentoId,
                                            request.CpfColaborador,
                                            request.CargoColaborador,
                                            request.TelefoneColaborador,
                                            request.EmailPessoalColaborador,
                                            request.EmailCorporativo,
                                            request.Usuario);





            await _repositoryUsuario.EditarAsync(Colaborador.Usuario, cancellationToken);
            Colaborador = await _repositoryColaborador.EditarAsync(Colaborador, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Colaborador);

            return response;
        }
    }
}

