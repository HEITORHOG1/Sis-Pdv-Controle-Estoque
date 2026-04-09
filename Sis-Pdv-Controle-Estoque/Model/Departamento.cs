using Model.Exceptions;

namespace Model
{
    public class Departamento : EntityBase
    {
        public Departamento()
        {
        }

        public Departamento(string nomeDepartamento)
        {
            ValidarNomeDepartamento(nomeDepartamento);
            NomeDepartamento = nomeDepartamento;
        }

        public string NomeDepartamento { get; set; } = string.Empty;

        public void AlterarDepartamento(Guid id, string nomeDepartamento)
        {
            ValidarNomeDepartamento(nomeDepartamento);
            Id = id;
            NomeDepartamento = nomeDepartamento;
        }

        private static void ValidarNomeDepartamento(string nomeDepartamento)
        {
            if (string.IsNullOrWhiteSpace(nomeDepartamento))
                throw new DomainException("O nome do departamento é obrigatório.");

            if (nomeDepartamento.Length < 2)
                throw new DomainException("O nome do departamento deve ter no mínimo 2 caracteres.");

            if (nomeDepartamento.Length > 100)
                throw new DomainException("O nome do departamento deve ter no máximo 100 caracteres.");
        }
    }
}
