using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("Usuario")]
    public class Usuario : EntityBase
    {
        public Usuario()
        {

        }
        public Usuario( string login, string senha, bool statusAtivo,Guid id)
        {
            Login = login;
            Senha = senha;
            this.statusAtivo = statusAtivo;
            this.Id = id;
        }
       
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool statusAtivo { get; set; }
    }
}
