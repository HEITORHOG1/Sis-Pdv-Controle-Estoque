using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{

    public class Categoria : EntityBase
    {
        public Categoria()
        {

        }

        public Categoria(string nomeCategoria)
        {
            NomeCategoria = nomeCategoria;
        }

        public Categoria(Guid id, string nomeCategoria)
        {
            Id = id;
            NomeCategoria = nomeCategoria;
        }
        public string NomeCategoria { get; set; }
        public Guid? Id { get; set; }

        public void AlterarCategoria(Guid id, string nomeCategoria)
        {
            Id = id;
            NomeCategoria = nomeCategoria;
        }
    }
}
