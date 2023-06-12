using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.RemoverColaborador
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
