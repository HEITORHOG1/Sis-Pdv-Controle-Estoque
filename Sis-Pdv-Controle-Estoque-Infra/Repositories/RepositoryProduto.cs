using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryProduto : RepositoryBase<Produto, Guid>, IRepositoryProduto
    {
        public RepositoryProduto(PdvContext context) : base(context)
        {
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Produto>().CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Produto>> GetLowStockProductsAsync(CancellationToken cancellationToken = default)
        {
             return await _context.Set<Produto>()
                .Where(p => p.QuantidadeEstoqueProduto <= p.MinimumStock && !p.IsDeleted && p.StatusAtivo == 1)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<Produto> ListarTodosAtivos(params Expression<Func<Produto, object>>[] includeProperties)
        {
             IQueryable<Produto> query = _context.Set<Produto>().Where(x => !x.IsDeleted && x.StatusAtivo == 1);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        public IQueryable<Produto> ListarInativos(params Expression<Func<Produto, object>>[] includeProperties)
        {
             IQueryable<Produto> query = _context.Set<Produto>().Where(x => x.StatusAtivo == 0 && !x.IsDeleted);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        public void Desativar(Produto produto)
        {
            produto.StatusAtivo = 0;
            _context.Entry(produto).State = EntityState.Modified;
        }

        public void Ativar(Produto produto)
        {
            produto.StatusAtivo = 1;
            _context.Entry(produto).State = EntityState.Modified;
        }

        public async Task<bool> DesativarAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var produto = await _context.Set<Produto>().FindAsync(new object[] { id }, cancellationToken);
            if (produto != null)
            {
                produto.StatusAtivo = 0;
                _context.Entry(produto).State = EntityState.Modified;
                return true;
            }
            return false;
        }

        public async Task<bool> AtivarAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var produto = await _context.Set<Produto>().FindAsync(new object[] { id }, cancellationToken);
            if (produto != null)
            {
                produto.StatusAtivo = 1;
                _context.Entry(produto).State = EntityState.Modified;
                return true;
            }
            return false;
        }
    }
}
