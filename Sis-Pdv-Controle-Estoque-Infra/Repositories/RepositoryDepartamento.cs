using Repositories.Base;
namespace Repositories
{
    public class RepositoryDepartamento : RepositoryBase<Departamento, Guid>, IRepositoryDepartamento
    {
        public RepositoryDepartamento(PdvContext context) : base(context)
        {
        }
    }
}
