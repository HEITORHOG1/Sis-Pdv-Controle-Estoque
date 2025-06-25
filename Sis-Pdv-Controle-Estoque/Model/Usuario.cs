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
            StatusAtivo = statusAtivo;
            Id = id;
        }

        public string Login { get; set; }
        public string Senha { get; set; }
        public bool StatusAtivo { get; set; }
    }
}
