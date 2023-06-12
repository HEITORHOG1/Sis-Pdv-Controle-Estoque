using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Departamento
{
    public  class DepartamentoResponse
    {
        public List<object> notifications { get; set; }
        public bool success { get; set; }
        public Data data { get; set; }
    }
}


