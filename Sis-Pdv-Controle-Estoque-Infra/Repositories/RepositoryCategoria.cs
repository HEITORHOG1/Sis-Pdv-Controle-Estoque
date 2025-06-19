using Repositories.Base;

namespace Repositories
{
    public class RepositoryCategoria : RepositoryBase<Categoria, Guid>, IRepositoryCategoria
    {
        private readonly PdvContext _context;
        public RepositoryCategoria(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
