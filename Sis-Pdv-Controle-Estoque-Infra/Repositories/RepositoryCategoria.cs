using Repositories.Base;

namespace Repositories
{
    public class RepositoryCategoria : RepositoryBase<Categoria, Guid>, IRepositoryCategoria
    {
        public RepositoryCategoria(PdvContext context) : base(context)
        {
        }
    }
}
