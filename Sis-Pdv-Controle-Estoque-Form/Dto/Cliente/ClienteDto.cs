namespace Sis_Pdv_Controle_Estoque_Form.Dto.Cliente
{

    public class ClienteDto
    {
        public ClienteDto()
        {

        }

        public ClienteDto(Guid id, string CpfCnpj, string tipoCliente)
        {
            this.id = id;
            CpfCnpj = CpfCnpj;
            this.tipoCliente = tipoCliente;
        }

        public Guid id { get; set; }
        public string CpfCnpj { get; set; }
        public string tipoCliente { get; set; }

        public static implicit operator ClienteDto(ClienteResponse dto)
        {
            return new ClienteDto()
            {
                id = dto.data.id,
                tipoCliente = dto.data.tipoCliente,
                CpfCnpj = dto.data.CpfCnpj

            };

        }
    }
}
