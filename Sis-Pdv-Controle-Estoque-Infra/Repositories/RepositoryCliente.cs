using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryCliente : RepositoryBase<Cliente, Guid>, IRepositoryCliente
    {
        public RepositoryCliente(PdvContext context) : base(context)
        {
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Cliente>().CountAsync(cancellationToken);
        }
    }
}
