using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.ListarCategoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeCategoria { get; set; }

        public static explicit operator ListarCategoriaPorIdResponse(Sis_Pdv_Controle_Estoque.Model.Categoria cat)
        {
            return new ListarCategoriaPorIdResponse()
            {
                Id = cat.Id,
                NomeCategoria = cat.NomeCategoria
            };
        }
    }
}
