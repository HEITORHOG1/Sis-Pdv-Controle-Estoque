namespace Commands.Departamento.ListarDepartamentoPorId
{
    public class ListarDepartamentoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeDepartamento { get; set; }

        public static explicit operator ListarDepartamentoPorIdResponse(Sis_Pdv_Controle_Estoque.Model.Departamento cat)
        {
            return new ListarDepartamentoPorIdResponse()
            {
                Id = cat.Id,
                NomeDepartamento = cat.NomeDepartamento
            };
        }
    }
}
