namespace Repositories
{
    public class RepositoryFornecedor : RepositoryBase<Fornecedor, Guid>, IRepositoryFornecedor
    {
        private readonly PdvContext _context;
        public RepositoryFornecedor(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
