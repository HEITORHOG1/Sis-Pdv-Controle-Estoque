using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interfaces.Repositories.Base
{
    public interface IRepositoryBase<TEntidade, TId>
    where TEntidade : class
    where TId : struct
    {
        IQueryable<TEntidade> ListarPor(Expression<Func<TEntidade, bool>> where, params Expression<Func<TEntidade, object>>[] includeProperties);

        IQueryable<TEntidade> ListarEOrdenadosPor<TKey>(Expression<Func<TEntidade, bool>> where, Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntidade, object>>[] includeProperties);

        TEntidade ObterPor(Expression<Func<TEntidade, bool>> where, params Expression<Func<TEntidade, object>>[] includeProperties);

        Task<TEntidade?> ObterPorAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);

        bool Existe(Expression<Func<TEntidade, bool>> where);

        Task<bool> ExisteAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default);

        IQueryable<TEntidade> Listar(params Expression<Func<TEntidade, object>>[] includeProperties);

        Task<IEnumerable<TEntidade>> ListarAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);

        Task<IEnumerable<TEntidade>> ListarPorAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);

        IQueryable<TEntidade> ListarOrdenadosPor<TKey>(Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntidade, object>>[] includeProperties);

        Task<IEnumerable<TEntidade>> ListarOrdenadosPorAsync<TKey>(Expression<Func<TEntidade, TKey>> ordem, bool ascendente = true, CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);

        TEntidade ObterPorId(TId id, params Expression<Func<TEntidade, object>>[] includeProperties);

        Task<TEntidade?> ObterPorIdAsync(TId id, CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);

        TEntidade Adicionar(TEntidade entidade);

        Task<TEntidade> AdicionarAsync(TEntidade entidade, CancellationToken cancellationToken = default);

        TEntidade Editar(TEntidade entidade);

        Task<TEntidade> EditarAsync(TEntidade entidade, CancellationToken cancellationToken = default);

        void Remover(TEntidade entidade);

        void Remover(IEnumerable<TEntidade> entidades);

        Task RemoverAsync(TEntidade entidade, CancellationToken cancellationToken = default);

        Task RemoverAsync(IEnumerable<TEntidade> entidades, CancellationToken cancellationToken = default);

        Task RemoverAsync(TId id, CancellationToken cancellationToken = default);

        void AdicionarLista(IEnumerable<TEntidade> entidades);

        Task AdicionarListaAsync(IEnumerable<TEntidade> entidades, CancellationToken cancellationToken = default);

        Task<int> ContarAsync(Expression<Func<TEntidade, bool>>? where = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntidade>> ListarPaginadoAsync(int pageNumber, int pageSize, Expression<Func<TEntidade, bool>>? where = null, CancellationToken cancellationToken = default, params Expression<Func<TEntidade, object>>[] includeProperties);
        
        // Enhanced methods for inventory management
        Task<TEntidade?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntidade>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntidade>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
        Task<TEntidade> AddAsync(TEntidade entity, CancellationToken cancellationToken = default);
        Task<TEntidade> UpdateAsync(TEntidade entity, CancellationToken cancellationToken = default);

        // Soft delete methods
        IQueryable<TEntidade> ListarTodos(params Expression<Func<TEntidade, object>>[] includeProperties);
        IQueryable<TEntidade> ListarDeletados(params Expression<Func<TEntidade, object>>[] includeProperties);
        void RemoverFisicamente(TEntidade entidade);
        void RemoverFisicamente(IEnumerable<TEntidade> entidades);
        void Restaurar(TEntidade entidade);
        Task<bool> RestaurarAsync(TId id, CancellationToken cancellationToken = default);
        void Restaurar(IEnumerable<TEntidade> entidades);
    }
}
