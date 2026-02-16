namespace Model
{
    public class Colaborador : EntityBase
    {

        public Colaborador()
        {
            Usuario = new Usuario();
            Departamento = new Departamento();
        }
        public Colaborador(Guid id, string nomeColaborador,
            Guid? DepartamentoId, string cpfColaborador,
            string cargoColaborador, string telefoneColaborador,
            string emailPessoalColaborador, string emailCorporativo, Usuario usuario)
        {
            NomeColaborador = nomeColaborador;
            this.DepartamentoId = DepartamentoId;
            CpfColaborador = cpfColaborador;
            CargoColaborador = cargoColaborador;
            TelefoneColaborador = telefoneColaborador;
            EmailPessoalColaborador = emailPessoalColaborador;
            EmailCorporativo = emailCorporativo;
            Usuario = usuario;
            Id = id;
        }

        public string NomeColaborador { get; set; }
        public virtual Guid? DepartamentoId { get; set; }
        public string CpfColaborador { get; set; }
        public string CargoColaborador { get; set; }
        public string TelefoneColaborador { get; set; }
        public string EmailPessoalColaborador { get; set; }
        public string EmailCorporativo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Departamento Departamento { get; set; }
        public void AlterarColaborador(Guid id, string nomeColaborador, Guid Departamento, string cpfColaborador, string cargoColaborador,
            string telefoneColaborador, string emailPessoalColaborador, string emailCorporativo, Usuario Usuario)
        {
            NomeColaborador = nomeColaborador;
            DepartamentoId = Departamento;
            CpfColaborador = cpfColaborador;
            CargoColaborador = cargoColaborador;
            TelefoneColaborador = telefoneColaborador;
            EmailPessoalColaborador = emailPessoalColaborador;
            EmailCorporativo = emailCorporativo;
            this.Usuario = Usuario;
            Id = id;
        }


    }
}
