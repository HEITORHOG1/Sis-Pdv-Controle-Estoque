using MessageBus;

namespace Model
{
    public class CupomDTO : BaseMessage
    {
        public string CodItem { get; set; }
        public string CodBarras { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string ValorUnit { get; set; }
        public string Total { get; set; }
        public string Status { get; set; }
        public string CPF { get; set; }
        public string TotalVendido { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Caixa { get; set; }
        public string FormaPagamento { get; set; }
        public string ValorRecebido { get; set; }
        public string Troco { get; set; }

        public CupomDTO(string codItem, string codBarras, string descricao, string quantidade, string valorUnit,
                     string total, string status, string cpf, string totalVendido, string data, string hora,
                     string caixa, string formaPagamento, string valorRecebido, string troco)
        {
            CodItem = codItem;
            CodBarras = codBarras;
            Descricao = descricao;
            Quantidade = quantidade;
            ValorUnit = valorUnit;
            Total = total;
            Status = status;
            CPF = cpf;
            TotalVendido = totalVendido;
            Data = data;
            Hora = hora;
            Caixa = caixa;
            FormaPagamento = formaPagamento;
            ValorRecebido = valorRecebido;
            Troco = troco;
        }
    }

}
