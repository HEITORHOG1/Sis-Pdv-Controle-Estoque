namespace Sis_Pdv_Controle_Estoque_Form.Dto.Categoria
{

    public class CategoriaDto
    {
        public CategoriaDto()
        {

        }
        public CategoriaDto(string NomeCategoria, Guid _id)
        {
            NomeCategoria = NomeCategoria;
            id = _id;
        }
        public string NomeCategoria { get; set; }
        public Guid id { get; set; }
    }
}
