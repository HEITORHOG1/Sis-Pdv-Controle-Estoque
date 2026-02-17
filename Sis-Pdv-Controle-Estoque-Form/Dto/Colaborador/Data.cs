using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador
{
    public class Data
    {
        public string NomeColaborador { get; set; }
        public string departamentoId { get; set; }
        public string cpfColaborador { get; set; }
        public string cargoColaborador { get; set; }
        public string telefoneColaborador { get; set; }
        public string emailPessoalColaborador { get; set; }
        public string emailCorporativo { get; set; }
        public Usuario usuario { get; set; }
        public DepartamentoDto Departamento { get; set; }
        public string id { get; set; }
    }
}
public class Usuario
{
    public string login { get; set; }
    public string senha { get; set; }
    public bool StatusAtivo { get; set; }
    public string id { get; set; }
}
