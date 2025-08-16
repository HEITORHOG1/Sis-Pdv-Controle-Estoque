namespace Interfaces
{
    public interface IRepositoryProduto : IRepositoryBase<Produto, Guid>
    {
        Task<int> CountAsync();
        Task<IEnumerable<Produto>> GetLowStockProductsAsync();
    }
}
