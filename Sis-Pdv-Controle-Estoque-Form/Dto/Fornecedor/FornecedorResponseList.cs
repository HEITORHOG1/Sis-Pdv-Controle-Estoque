namespace Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor
{
    public  class FornecedorResponseList
    {
        public List<object> notifications { get; set; }
        public bool success { get; set; }
        public List<Data> data { get; set; }
    }
}


