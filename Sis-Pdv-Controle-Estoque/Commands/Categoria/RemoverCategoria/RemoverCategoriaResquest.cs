using MediatR;

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
