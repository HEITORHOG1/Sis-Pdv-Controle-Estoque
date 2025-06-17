namespace Sis_Pdv_Controle_Estoque_Form.Dto.Categoria
{

    public class CategoriaDto
    {
        public CategoriaDto()
        {

        }
        public CategoriaDto(string nomeCategoria, Guid _id)
        {
            NomeCategoria = nomeCategoria;
            id = _id;
        }
        public string NomeCategoria { get; set; }
        public Guid id { get; set; }
    }
}
