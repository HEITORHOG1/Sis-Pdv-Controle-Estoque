namespace Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeColaborador { get; set; }

        public static explicit operator ListarColaboradorPorIdResponse(Model.Colaborador cat)
        {
            return new ListarColaboradorPorIdResponse()
            {
                Id = cat.Id,
                NomeColaborador = cat.NomeColaborador
            };
        }
    }
}
