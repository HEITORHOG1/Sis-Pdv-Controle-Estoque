using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepositoryProduto : IRepositoryBase<Produto, Guid>
    {
        Task<int> CountAsync();
        Task<IEnumerable<Produto>> GetLowStockProductsAsync();
        
        // Métodos específicos para controle de status
        IQueryable<Produto> ListarTodosAtivos(params Expression<Func<Produto, object>>[] includeProperties);
        IQueryable<Produto> ListarInativos(params Expression<Func<Produto, object>>[] includeProperties);
        void Desativar(Produto produto);
        void Ativar(Produto produto);
        Task<bool> DesativarAsync(Guid id);
        Task<bool> AtivarAsync(Guid id);
    }
}
