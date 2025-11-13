namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
    }
}