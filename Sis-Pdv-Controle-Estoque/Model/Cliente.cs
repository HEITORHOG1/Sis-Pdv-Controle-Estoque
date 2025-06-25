namespace Model
{
    public class Cliente : EntityBase
    {
        public Cliente()
        {

        }

        public Cliente(string cpfCnpj, string tipoCliente)
        {
            CpfCnpj = cpfCnpj;
            TipoCliente = tipoCliente;
        }
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
    }
}
