using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Cliente")]
    public class Cliente : EntityBase
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
