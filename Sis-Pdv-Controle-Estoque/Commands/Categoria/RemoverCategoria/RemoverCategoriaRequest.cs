using MediatR;

namespace Commands.Categoria.RemoverCategoria
{
    public class RemoverCategoriaRequest : IRequest<Response>
    {
        public RemoverCategoriaRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
