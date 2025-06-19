using Repositories.Base;

namespace Repositories
{
    public class RepositoryUsuario : RepositoryBase<Usuario, Guid>, IRepositoryUsuario
    {
        private readonly PdvContext _context;
        public RepositoryUsuario(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
