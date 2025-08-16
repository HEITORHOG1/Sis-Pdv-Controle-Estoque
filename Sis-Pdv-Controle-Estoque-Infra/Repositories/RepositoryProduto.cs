using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryProduto : RepositoryBase<Produto, Guid>, IRepositoryProduto
    {
        private readonly PdvContext _context;
        public RepositoryProduto(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Produtos.CountAsync();
        }

        public async Task<IEnumerable<Produto>> GetLowStockProductsAsync()
        {
            return await _context.Produtos
                .Where(p => p.QuatidadeEstoqueProduto <= p.MinimumStock)
                .ToListAsync();
        }
    }
}
