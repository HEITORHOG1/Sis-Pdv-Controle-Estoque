namespace Repositories
{
    public class RepositoryProduto : RepositoryBase<Produto, Guid>, IRepositoryProduto
    {
        private readonly PdvContext _context;
        public RepositoryProduto(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
