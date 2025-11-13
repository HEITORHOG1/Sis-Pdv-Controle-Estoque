using Interfaces;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryUsuario _userRepository;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IRepositoryUsuario userRepository, ILogger<PermissionService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            try
            {
                return await _userRepository.HasPermissionAsync(userId, permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission {Permission} for user {UserId}", permission, userId);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
        {
            try
            {
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                return permissions.Select(p => p.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions for user {UserId}", userId);
                return Enumerable.Empty<string>();
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            try
            {
                var roles = await _userRepository.GetUserRolesAsync(userId);
                return roles.Select(r => r.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {UserId}", userId);
                return Enumerable.Empty<string>();
            }
        }
    }
}