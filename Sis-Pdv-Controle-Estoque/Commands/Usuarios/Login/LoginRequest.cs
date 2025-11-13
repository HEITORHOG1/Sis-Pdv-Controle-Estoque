using MediatR;

namespace Commands.Usuarios.Login
{
    public class LoginRequest : IRequest<Response>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}