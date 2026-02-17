using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;

namespace Sis_Pdv_Controle_Estoque_Form.IServices
{
    public interface ICategoriaService
    {
        Task<CategoriaResponse> Adicionar(CategoriaDto dto);
        Task<CategoriaResponseList> ListarCategoria();
        Task<CategoriaResponseList> ListarCategoriaPorId(string id);
        Task<CategoriaResponseList> ListarCategoriaPorNomeCategoria(string NomeCategoria);
        Task<CategoriaResponse> AlterarCategoria(CategoriaDto dto);
        Task<CategoriaResponse> RemoverCategoria(string id);
    }
}
