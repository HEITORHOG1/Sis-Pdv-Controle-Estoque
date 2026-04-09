using Model.Base;
using Model.Exceptions;

namespace Model
{
    public class Categoria : EntityBase
    {
        public Categoria()
        {
        }

        public Categoria(string nomeCategoria)
        {
            ValidarNomeCategoria(nomeCategoria);
            NomeCategoria = nomeCategoria;
        }

        public Categoria(Guid id, string nomeCategoria)
        {
            ValidarNomeCategoria(nomeCategoria);
            Id = id;
            NomeCategoria = nomeCategoria;
        }

        public string NomeCategoria { get; set; } = string.Empty;

        public void AlterarCategoria(Guid id, string nomeCategoria)
        {
            ValidarNomeCategoria(nomeCategoria);
            Id = id;
            NomeCategoria = nomeCategoria;
        }

        private static void ValidarNomeCategoria(string nomeCategoria)
        {
            if (string.IsNullOrWhiteSpace(nomeCategoria))
                throw new DomainException("O nome da categoria é obrigatório.");

            if (nomeCategoria.Length < 2)
                throw new DomainException("O nome da categoria deve ter no mínimo 2 caracteres.");

            if (nomeCategoria.Length > 100)
                throw new DomainException("O nome da categoria deve ter no máximo 100 caracteres.");
        }
    }
}
