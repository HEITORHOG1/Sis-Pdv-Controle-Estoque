using System.Threading.Tasks;

namespace Repositories.Transactions
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
