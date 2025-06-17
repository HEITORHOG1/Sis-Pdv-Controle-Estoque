namespace Interfaces
{
    public interface IRepositoryPedido : IRepositoryBase<Pedido, Guid>
    {
        Task<IList<ListarVendaPedidoPorDataResponse>> ListarVendaPedidoPorData(DateTime DataInicio, DateTime DataFim);
    }
}
