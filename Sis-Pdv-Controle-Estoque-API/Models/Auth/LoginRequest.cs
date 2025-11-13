using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Models.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Login é obrigatório")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; } = string.Empty;
    }
}