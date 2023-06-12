using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador
{
    public class ColaboradorDto
    {
        public string id { get; set; }
        public string nomeColaborador { get; set; }
        public string departamentoId { get; set; }
        public string cpfColaborador { get; set; }
        public string cargoColaborador { get; set; }
        public string telefoneColaborador { get; set; }
        public string emailPessoalColaborador { get; set; }
        public string emailCorporativo { get; set; }
        public string idlogin { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public bool statusAtivo { get; set; }

    }
}
