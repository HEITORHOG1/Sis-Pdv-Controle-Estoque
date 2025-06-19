using Repositories.Base;
namespace Repositories
{
    public class RepositoryColaborador : RepositoryBase<Colaborador, Guid>, IRepositoryColaborador
    {
        private readonly PdvContext _context;
        public RepositoryColaborador(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
