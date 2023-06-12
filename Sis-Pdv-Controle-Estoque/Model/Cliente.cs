using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("Cliente")]
    public  class Cliente : EntityBase
    {
        public Cliente()
        {
                
        }

        public Cliente(string cpfCnpj, string tipoCliente)
        {
            CpfCnpj = cpfCnpj;
            this.tipoCliente = tipoCliente;
        }
        public string CpfCnpj { get; set; }
        public string tipoCliente { get; set; }
    }
}
