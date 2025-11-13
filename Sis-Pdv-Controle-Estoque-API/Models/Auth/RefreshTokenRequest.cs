using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Models.Auth
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}