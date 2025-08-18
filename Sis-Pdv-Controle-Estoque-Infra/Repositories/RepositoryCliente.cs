using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryCliente : RepositoryBase<Cliente, Guid>, IRepositoryCliente
    {
        private readonly PdvContext _context;
        public RepositoryCliente(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Clientes.CountAsync();
        }
    }
}
