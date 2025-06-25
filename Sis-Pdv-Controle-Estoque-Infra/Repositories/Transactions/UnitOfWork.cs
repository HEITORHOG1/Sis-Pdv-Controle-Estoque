using Repositories.Base;
using System.Threading.Tasks;

namespace Repositories.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PdvContext _context;

        public UnitOfWork(PdvContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
