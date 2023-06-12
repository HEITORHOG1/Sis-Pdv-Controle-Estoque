using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Categoria
{
    public class CategoriaResponseList
    {
        public List<object> notifications { get; set; }
        public List<CategoriaDto> data { get; set; }
        public bool success { get; set; }
    }
}
