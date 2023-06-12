using Sis_Pdv_Controle_Estoque_Infra.Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Transactions
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
    }
}
