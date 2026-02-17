using Repositories.Base;
namespace Repositories
{
    public class RepositoryColaborador : RepositoryBase<Colaborador, Guid>, IRepositoryColaborador
    {
        public RepositoryColaborador(PdvContext context) : base(context)
        {
        }
    }
}
