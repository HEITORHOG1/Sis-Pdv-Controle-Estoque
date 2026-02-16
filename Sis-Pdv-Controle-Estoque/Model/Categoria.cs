using Model.Base;

namespace Model
{

    public class Categoria : EntityBase
    {
        public Categoria()
        {

        }

        public Categoria(string nomeCategoria)
        {
            NomeCategoria = nomeCategoria;
        }

        public Categoria(Guid id, string nomeCategoria)
        {
            Id = id;
            NomeCategoria = nomeCategoria;
        }
        public string NomeCategoria { get; set; } = string.Empty;

        public void AlterarCategoria(Guid id, string nomeCategoria)
        {
            Id = id;
            NomeCategoria = nomeCategoria;
        }
    }
}
