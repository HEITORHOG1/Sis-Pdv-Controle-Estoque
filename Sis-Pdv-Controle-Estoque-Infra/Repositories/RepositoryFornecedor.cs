using Repositories.Base;
namespace Repositories
{
    public class RepositoryFornecedor : RepositoryBase<Fornecedor, Guid>, IRepositoryFornecedor
    {
        public RepositoryFornecedor(PdvContext context) : base(context)
        {
        }
    }
}
