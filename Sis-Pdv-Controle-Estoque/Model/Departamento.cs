namespace Model
{
    public class Departamento : EntityBase
    {
        public Departamento()
        {

        }
        public Departamento(string NomeDepartamento)
        {
            this.NomeDepartamento = NomeDepartamento;
        }

        public string NomeDepartamento { get; set; }

        public void AlterarDepartamento(Guid id, string NomeDepartamento)
        {
            Id = id;
            this.NomeDepartamento = NomeDepartamento;
        }
    }
}
