using Model.Base;

namespace Model
{
    public class Usuario : EntityBase
    {
        public Usuario()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Usuario(string login, string senha, bool StatusAtivo, Guid id)
        {
            Login = login;
            Senha = senha;
            this.StatusAtivo = StatusAtivo;
            Id = id;
            UserRoles = new HashSet<UserRole>();
        }

        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Nome { get; set; }
        public bool StatusAtivo { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
