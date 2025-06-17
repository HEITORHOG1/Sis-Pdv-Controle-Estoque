using MediatR;

namespace Commands.Colaborador.RemoverColaborador
{
    public class RemoverColaboradorResquest : IRequest<Response>
    {
        public RemoverColaboradorResquest(Guid id)
        {
            Id = id;
            //IdLogin = idLogin;
        }

        public Guid Id { get; set; }
        //public Guid IdLogin { get; set; }
    }
}
