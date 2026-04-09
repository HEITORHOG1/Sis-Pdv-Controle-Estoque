using Repositories.Base;

namespace Repositories
{
    public class RepositoryCupom : RepositoryBase<Cupom, Guid>, IRepositoryCupom
    {
        public RepositoryCupom(PdvContext context) : base(context)
        {
        }
    }
}
