namespace Commands.Departamento.ListarDepartamentoPorId
{
    public class ListarDepartamentoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeDepartamento { get; set; }

        public static explicit operator ListarDepartamentoPorIdResponse(Model.Departamento cat)
        {
            return new ListarDepartamentoPorIdResponse()
            {
                Id = cat.Id,
                NomeDepartamento = cat.NomeDepartamento
            };
        }
    }
}
