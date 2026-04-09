using Model.Exceptions;

namespace Model
{
    public class Colaborador : EntityBase
    {
        public Colaborador()
        {
        }

        public Colaborador(Guid id, string nomeColaborador,
            Guid? departamentoId, string cpfColaborador,
            string cargoColaborador, string telefoneColaborador,
            string emailPessoalColaborador, string emailCorporativo, Usuario usuario)
        {
            ValidarNomeColaborador(nomeColaborador);
            ValidarCpfColaborador(cpfColaborador);

            NomeColaborador = nomeColaborador;
            DepartamentoId = departamentoId;
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
        public void AlterarColaborador(Guid id, string nomeColaborador, Guid departamento, string cpfColaborador, string cargoColaborador,
            string telefoneColaborador, string emailPessoalColaborador, string emailCorporativo, Usuario usuario)
        {
            ValidarNomeColaborador(nomeColaborador);
            ValidarCpfColaborador(cpfColaborador);

            NomeColaborador = nomeColaborador;
            DepartamentoId = departamento;
            CpfColaborador = cpfColaborador;
            CargoColaborador = cargoColaborador;
            TelefoneColaborador = telefoneColaborador;
            EmailPessoalColaborador = emailPessoalColaborador;
            EmailCorporativo = emailCorporativo;
            Usuario = usuario;
            Id = id;
        }

        private static void ValidarNomeColaborador(string nomeColaborador)
        {
            if (string.IsNullOrWhiteSpace(nomeColaborador))
                throw new DomainException("O nome do colaborador é obrigatório.");

            if (nomeColaborador.Length < 2)
                throw new DomainException("O nome do colaborador deve ter no mínimo 2 caracteres.");
        }

        private static void ValidarCpfColaborador(string cpfColaborador)
        {
            if (string.IsNullOrWhiteSpace(cpfColaborador))
                throw new DomainException("O CPF do colaborador é obrigatório.");
        }
    }
}
