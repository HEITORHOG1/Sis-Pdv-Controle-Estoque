using Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public class RepositoryProduto : RepositoryBase<Produto, Guid>, IRepositoryProduto
    {
        private readonly PdvContext _context;
        public RepositoryProduto(PdvContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Override do método Listar para filtrar produtos ativos e não deletados
        /// </summary>
        public override IQueryable<Produto> Listar(params Expression<Func<Produto, object>>[] includeProperties)
        {
            IQueryable<Produto> query = _context.Set<Produto>()
                .Where(x => !x.IsDeleted && x.StatusAtivo == 1);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista produtos incluindo inativos mas excluindo deletados
        /// </summary>
        public IQueryable<Produto> ListarTodosAtivos(params Expression<Func<Produto, object>>[] includeProperties)
        {
            IQueryable<Produto> query = _context.Set<Produto>().Where(x => !x.IsDeleted);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista apenas produtos inativos (StatusAtivo = 0) mas não deletados
        /// </summary>
        public IQueryable<Produto> ListarInativos(params Expression<Func<Produto, object>>[] includeProperties)
        {
            IQueryable<Produto> query = _context.Set<Produto>()
                .Where(x => !x.IsDeleted && x.StatusAtivo == 0);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Desativa um produto (StatusAtivo = 0) sem deletá-lo
        /// </summary>
        public void Desativar(Produto produto)
        {
            produto.StatusAtivo = 0;
            produto.UpdatedAt = DateTime.UtcNow;
            _context.Entry(produto).State = EntityState.Modified;
        }

        /// <summary>
        /// Ativa um produto (StatusAtivo = 1)
        /// </summary>
        public void Ativar(Produto produto)
        {
            produto.StatusAtivo = 1;
            produto.UpdatedAt = DateTime.UtcNow;
            _context.Entry(produto).State = EntityState.Modified;
        }

        /// <summary>
        /// Desativa um produto pelo ID
        /// </summary>
        public async Task<bool> DesativarAsync(Guid id)
        {
            var produto = await ObterPorIdAsync(id);
            if (produto != null)
            {
                Desativar(produto);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ativa um produto pelo ID
        /// </summary>
        public async Task<bool> AtivarAsync(Guid id)
        {
            var produto = await ListarTodosAtivos().FirstOrDefaultAsync(x => x.Id == id);
            if (produto != null)
            {
                Ativar(produto);
                return true;
            }
            return false;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Produtos
                .Where(p => !p.IsDeleted && p.StatusAtivo == 1)
                .CountAsync();
        }

        public async Task<IEnumerable<Produto>> GetLowStockProductsAsync()
        {
            // Low stock logic moved to InventoryBalance domain
            // This method should be updated to use InventoryBalance or removed
            return await _context.Produtos
                .Where(p => !p.IsDeleted && p.StatusAtivo == 1)
                .ToListAsync();
        }

        /// <summary>
        /// Método auxiliar para Include (precisa ser protegido para override funcionar)
        /// </summary>
        protected new IQueryable<Produto> Include(IQueryable<Produto> query, params Expression<Func<Produto, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            return query;
        }
    }
}
