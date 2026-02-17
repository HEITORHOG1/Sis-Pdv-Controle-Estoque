using MediatR;

namespace Commands.Colaborador.RemoverColaborador
{
    public class RemoverColaboradorRequest : IRequest<Response>
    {
        public RemoverColaboradorRequest(Guid id)
        {
            Id = id;
            //IdLogin = idLogin;
        }

        public Guid Id { get; set; }
        //public Guid IdLogin { get; set; }
    }
}
