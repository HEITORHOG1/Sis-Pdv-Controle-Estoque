using Microsoft.EntityFrameworkCore;
using Model.Base;
using System.Linq.Expressions;

namespace Repositories.Base
{
    public class RepositoryBase<TEntidade, TId> : IRepositoryBase<TEntidade, TId>
      where TEntidade : EntityBase
      where TId : struct
    {

        protected readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntidade> ListarPor(Expression<Func<TEntidade, bool>> where, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return Listar(includeProperties).Where(where);
        }

        public IQueryable<TEntidade> ListarEOrdenadosPor<TKey>(Expression<Func<TEntidade, bool>> where, Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return ascendente ? ListarPor(where, includeProperties).OrderBy(ordem) : ListarPor(where, includeProperties).OrderByDescending(ordem);
        }

        public TEntidade ObterPor(Func<TEntidade, bool> where, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return Listar(includeProperties).FirstOrDefault(where);
        }

        public async Task<TEntidade?> ObterPorAsync(Expression<Func<TEntidade, bool>> where, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return await Listar(includeProperties).FirstOrDefaultAsync(where);
        }

        public TEntidade ObterPorId(TId id, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            if (includeProperties.Any())
            {
                return Listar(includeProperties).FirstOrDefault(x => x.Id.ToString() == id.ToString());
            }

            return _context.Set<TEntidade>().Find(id);
        }

        public async Task<TEntidade?> ObterPorIdAsync(TId id, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            if (includeProperties.Any())
            {
                return await Listar(includeProperties).FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString());
            }

            return await _context.Set<TEntidade>().FindAsync(id);
        }

        public IQueryable<TEntidade> Listar(params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            IQueryable<TEntidade> query = _context.Set<TEntidade>();

            if (includeProperties.Any())
            {
                return Include(_context.Set<TEntidade>(), includeProperties);
            }

            return query;
        }

        public async Task<IEnumerable<TEntidade>> ListarAsync(params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return await Listar(includeProperties).ToListAsync();
        }

        public IQueryable<TEntidade> ListarOrdenadosPor<TKey>(Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return ascendente ? Listar(includeProperties).OrderBy(ordem) : Listar(includeProperties).OrderByDescending(ordem);
        }

        public async Task<IEnumerable<TEntidade>> ListarOrdenadosPorAsync<TKey>(Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            return await (ascendente ? Listar(includeProperties).OrderBy(ordem) : Listar(includeProperties).OrderByDescending(ordem)).ToListAsync();
        }

        public TEntidade Adicionar(TEntidade entidade)
        {
            var entity = _context.Add(entidade);
            return entity.Entity;
        }

        public async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            var entity = await _context.AddAsync(entidade);
            return entity.Entity;
        }

        public TEntidade Editar(TEntidade entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            return entidade;
        }

        public async Task<TEntidade> EditarAsync(TEntidade entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            return await Task.FromResult(entidade);
        }

        public void Remover(TEntidade entidade)
        {
            _context.Set<TEntidade>().Remove(entidade);
        }

        public void Remover(IEnumerable<TEntidade> entidades)
        {
            _context.Set<TEntidade>().RemoveRange(entidades);
        }

        public async Task RemoverAsync(TEntidade entidade)
        {
            _context.Set<TEntidade>().Remove(entidade);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(IEnumerable<TEntidade> entidades)
        {
            _context.Set<TEntidade>().RemoveRange(entidades);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(TId id)
        {
            var entity = await ObterPorIdAsync(id);
            if (entity != null)
            {
                _context.Set<TEntidade>().Remove(entity);
            }
        }

        public void AdicionarLista(IEnumerable<TEntidade> entidades)
        {
            _context.AddRange(entidades);
        }

        public async Task AdicionarListaAsync(IEnumerable<TEntidade> entidades)
        {
            await _context.AddRangeAsync(entidades);
        }

        public bool Existe(Func<TEntidade, bool> where)
        {
            return _context.Set<TEntidade>().Any(where);
        }

        public async Task<bool> ExisteAsync(Expression<Func<TEntidade, bool>> where)
        {
            return await _context.Set<TEntidade>().AnyAsync(where);
        }

        public async Task<int> ContarAsync(Expression<Func<TEntidade, bool>>? where = null)
        {
            if (where != null)
            {
                return await _context.Set<TEntidade>().CountAsync(where);
            }
            return await _context.Set<TEntidade>().CountAsync();
        }

        public async Task<IEnumerable<TEntidade>> ListarPaginadoAsync(int pageNumber, int pageSize, Expression<Func<TEntidade, bool>>? where = null, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            var query = Listar(includeProperties);
            
            if (where != null)
            {
                query = query.Where(where);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Enhanced methods for inventory management
        public async Task<TEntidade?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntidade>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<TEntidade>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntidade>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntidade>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            var guidIds = ids.Select(id => Guid.Parse(id.ToString())).ToList();
            return await _context.Set<TEntidade>().Where(e => guidIds.Contains(e.Id)).ToListAsync(cancellationToken);
        }

        public async Task<TEntidade> AddAsync(TEntidade entity, CancellationToken cancellationToken = default)
        {
            var result = await _context.Set<TEntidade>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public async Task<TEntidade> UpdateAsync(TEntidade entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntidade>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Realiza include populando o objeto passado por parametro
        /// </summary>
        /// <param name="query">Informe o objeto do tipo IQuerable</param>
        /// <param name="includeProperties">Ínforme o array de expressões que deseja incluir</param>
        /// <returns></returns>
        private IQueryable<TEntidade> Include(IQueryable<TEntidade> query, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }
    }
}