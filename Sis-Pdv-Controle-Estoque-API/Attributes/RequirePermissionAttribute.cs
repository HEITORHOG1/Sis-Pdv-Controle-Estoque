using Microsoft.AspNetCore.Authorization;

namespace Sis_Pdv_Controle_Estoque_API.Attributes
{
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string permission)
        {
            Policy = $"Permission:{permission}";
        }
    }
}