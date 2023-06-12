using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Commands.Categoria.RemoverCategoria
{
    public class RemoverCategoriaResquest : IRequest<Response>
    {
        public RemoverCategoriaResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
