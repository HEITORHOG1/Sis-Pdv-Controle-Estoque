using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sis.Pdv.Blazor.Data.Entities;

/// <summary>
/// Entidade: Usuário autenticado salvo no banco local (Para login Offline).
/// </summary>
[Table("Usuarios")]
public class UsuarioLocalEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Login { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PasswordHashOffline { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    public string Roles { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
    public DateTime DataUltimoLogin { get; set; } = DateTime.UtcNow;
}
