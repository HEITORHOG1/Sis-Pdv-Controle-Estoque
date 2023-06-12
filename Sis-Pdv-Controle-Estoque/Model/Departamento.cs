using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("Departamento")]
    public  class Departamento : EntityBase
    {
        public Departamento()
        {
                
        }
        public Departamento(string NomeDepartamento)
        {
            this.NomeDepartamento = NomeDepartamento;
        }

        public string NomeDepartamento { get; set; }

        public void AlterarDepartamento(Guid id, string NomeDepartamento)
        {
            Id = id;
            this.NomeDepartamento = NomeDepartamento;
        }
    }
}
