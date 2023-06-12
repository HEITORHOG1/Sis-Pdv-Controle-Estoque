using Microsoft.EntityFrameworkCore;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryColaborador : RepositoryBase<Colaborador, Guid>, IRepositoryColaborador
    {
        private readonly PdvContext _context;
        public RepositoryColaborador(PdvContext context) : base(context)
        {
            _context = context;
        }
    }
}
