namespace Commands.Categoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeCategoria { get; set; }

        public static explicit operator ListarCategoriaPorIdResponse(Model.Categoria cat)
        {
            return new ListarCategoriaPorIdResponse()
            {
                Id = cat.Id,
                NomeCategoria = cat.NomeCategoria
            };
        }
    }
}
