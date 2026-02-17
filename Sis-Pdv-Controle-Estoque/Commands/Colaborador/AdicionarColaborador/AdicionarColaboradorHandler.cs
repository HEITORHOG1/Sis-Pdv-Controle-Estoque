using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.AdicionarColaborador
{
    public class AdicionarColaboradorHandler : Notifiable, IRequestHandler<AdicionarColaboradorRequest, Response>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public AdicionarColaboradorHandler(IRepositoryColaborador repositoryColaborador, IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Response> Handle(AdicionarColaboradorRequest request, CancellationToken cancellationToken)
        {



            Model.Colaborador Colaborador = new(
                                            request.Id,
                                            request.NomeColaborador,
                                            request.DepartamentoId,
                                            request.CpfColaborador,
                                            request.CargoColaborador,
                                            request.TelefoneColaborador,
                                            request.EmailPessoalColaborador,
                                            request.EmailCorporativo,
                                            request.Usuario);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Colaborador = await _repositoryColaborador.AdicionarAsync(Colaborador, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Colaborador);

            return response;
        }
    }
}

