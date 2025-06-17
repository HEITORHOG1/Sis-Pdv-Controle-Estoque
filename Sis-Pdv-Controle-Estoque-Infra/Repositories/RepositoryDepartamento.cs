namespace Repositories
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
