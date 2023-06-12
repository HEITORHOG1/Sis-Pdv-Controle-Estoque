using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryFornecedor : RepositoryBase<Fornecedor, Guid>, IRepositoryFornecedor
    {
        private readonly PdvContext _context;
        public RepositoryFornecedor(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
