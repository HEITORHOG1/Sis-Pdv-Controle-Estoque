namespace Interfaces
{
    public interface IRepositoryCliente : IRepositoryBase<Cliente, Guid>
    {
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
