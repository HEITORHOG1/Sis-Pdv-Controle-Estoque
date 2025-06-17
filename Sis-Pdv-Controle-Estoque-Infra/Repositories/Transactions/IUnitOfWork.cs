namespace Repositories.Transactions
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}
