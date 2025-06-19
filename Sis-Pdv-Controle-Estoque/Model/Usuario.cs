namespace Model
{
    public class Usuario : EntityBase
    {
        public Usuario()
        {

        }
        public Usuario(string login, string senha, bool statusAtivo, Guid id)
        {
            Login = login;
            Senha = senha;
            this.statusAtivo = statusAtivo;
            Id = id;
        }

        public string Login { get; set; }
        public string Senha { get; set; }
        public bool statusAtivo { get; set; }
    }
}
