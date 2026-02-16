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

        public virtual IQueryable<TEntidade> Listar(params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            IQueryable<TEntidade> query = _context.Set<TEntidade>().Where(x => !x.IsDeleted);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista todas as entidades incluindo as deletadas (soft delete)
        /// </summary>
        public IQueryable<TEntidade> ListarTodos(params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            IQueryable<TEntidade> query = _context.Set<TEntidade>();

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista apenas as entidades deletadas (soft delete)
        /// </summary>
        public IQueryable<TEntidade> ListarDeletados(params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            IQueryable<TEntidade> query = _context.Set<TEntidade>().Where(x => x.IsDeleted);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
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
            // Soft delete - marca como deletado ao invés de remover fisicamente
            entidade.IsDeleted = true;
            entidade.DeletedAt = DateTime.UtcNow;
            _context.Entry(entidade).State = EntityState.Modified;
        }

        public void Remover(IEnumerable<TEntidade> entidades)
        {
            // Soft delete para múltiplas entidades
            foreach (var entidade in entidades)
            {
                entidade.IsDeleted = true;
                entidade.DeletedAt = DateTime.UtcNow;
                _context.Entry(entidade).State = EntityState.Modified;
            }
        }

        public async Task RemoverAsync(TEntidade entidade)
        {
            // Soft delete assíncrono
            entidade.IsDeleted = true;
            entidade.DeletedAt = DateTime.UtcNow;
            _context.Entry(entidade).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(IEnumerable<TEntidade> entidades)
        {
            // Soft delete assíncrono para múltiplas entidades
            foreach (var entidade in entidades)
            {
                entidade.IsDeleted = true;
                entidade.DeletedAt = DateTime.UtcNow;
                _context.Entry(entidade).State = EntityState.Modified;
            }
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(TId id)
        {
            var entity = await ObterPorIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Remove fisicamente a entidade do banco de dados (hard delete)
        /// Use apenas quando necessário para limpeza de dados
        /// </summary>
        public void RemoverFisicamente(TEntidade entidade)
        {
            _context.Set<TEntidade>().Remove(entidade);
        }

        /// <summary>
        /// Remove fisicamente múltiplas entidades do banco de dados (hard delete)
        /// Use apenas quando necessário para limpeza de dados
        /// </summary>
        public void RemoverFisicamente(IEnumerable<TEntidade> entidades)
        {
            _context.Set<TEntidade>().RemoveRange(entidades);
        }

        /// <summary>
        /// Restaura uma entidade que foi deletada (soft delete)
        /// </summary>
        public void Restaurar(TEntidade entidade)
        {
            entidade.IsDeleted = false;
            entidade.DeletedAt = null;
            entidade.DeletedBy = null;
            _context.Entry(entidade).State = EntityState.Modified;
        }

        /// <summary>
        /// Restaura uma entidade que foi deletada pelo ID (soft delete)
        /// </summary>
        public async Task<bool> RestaurarAsync(TId id)
        {
            var entity = await ListarTodos().FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString() && x.IsDeleted);
            if (entity != null)
            {
                Restaurar(entity);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Restaura múltiplas entidades que foram deletadas (soft delete)
        /// </summary>
        public void Restaurar(IEnumerable<TEntidade> entidades)
        {
            foreach (var entidade in entidades)
            {
                entidade.IsDeleted = false;
                entidade.DeletedAt = null;
                entidade.DeletedBy = null;
                _context.Entry(entidade).State = EntityState.Modified;
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
            return result.Entity;
        }

        public async Task<TEntidade> UpdateAsync(TEntidade entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntidade>().Update(entity);
            return entity;
        }

        /// <summary>
        /// Realiza include populando o objeto passado por parametro
        /// </summary>
        /// <param name="query">Informe o objeto do tipo IQuerable</param>
        /// <param name="includeProperties">Ínforme o array de expressões que deseja incluir</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntidade> Include(IQueryable<TEntidade> query, params Expression<Func<TEntidade, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }
    }
}