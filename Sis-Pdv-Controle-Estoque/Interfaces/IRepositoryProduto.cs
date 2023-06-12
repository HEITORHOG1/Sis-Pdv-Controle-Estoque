﻿using Sis_Pdv_Controle_Estoque.Interfaces.Repositories.Base;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Interfaces
{
    public interface IRepositoryProduto : IRepositoryBase<Produto, Guid>
    {
    }
}
