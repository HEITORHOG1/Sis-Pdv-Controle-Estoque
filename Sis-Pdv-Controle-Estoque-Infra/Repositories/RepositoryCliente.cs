namespace Repositories
{
    public class RepositoryCliente : RepositoryBase<Cliente, Guid>, IRepositoryCliente
    {
        private readonly PdvContext _context;
        public RepositoryCliente(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
