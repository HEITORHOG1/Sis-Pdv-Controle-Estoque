using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryDepartamento : RepositoryBase<Departamento, Guid>, IRepositoryDepartamento
    {
        private readonly PdvContext _context;
        public RepositoryDepartamento(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
