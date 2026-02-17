using Model.Base;

namespace Model
{

    public class Categoria : EntityBase
    {
        public Categoria()
        {

        }

        public Categoria(string NomeCategoria)
        {
            this.NomeCategoria = NomeCategoria;
        }

        public Categoria(Guid id, string NomeCategoria)
        {
            Id = id;
            this.NomeCategoria = NomeCategoria;
        }
        public string NomeCategoria { get; set; } = string.Empty;

        public void AlterarCategoria(Guid id, string NomeCategoria)
        {
            Id = id;
            this.NomeCategoria = NomeCategoria;
        }
    }
}
