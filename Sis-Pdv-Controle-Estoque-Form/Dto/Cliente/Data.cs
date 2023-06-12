using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Cliente
{
    public  class Data
    {
        public Guid id { get; set; }
        public string CpfCnpj { get; set; }
        public string tipoCliente { get; set; }
    }
}
