namespace Sis_Pdv_Controle_Estoque_Form.Dto.Cliente
{
    public class ClienteResponseList
    {
        public List<object> notifications { get; set; }
        public List<ClienteDto> data { get; set; }
        public bool success { get; set; }
    }
}
