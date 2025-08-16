using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;
using Sis_Pdv_Controle_Estoque_API.Services.Cache;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CacheController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// Remove um item específico do cache
        /// </summary>
        /// <param name="key">Chave do cache a ser removida</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{key}")]
        public async Task<IActionResult> RemoveCache(string key)
        {
            await _cacheService.RemoveAsync(key);
            return Ok(new { message = $"Cache key '{key}' removed successfully" });
        }

        /// <summary>
        /// Remove itens do cache por padrão
        /// </summary>
        /// <param name="pattern">Padrão regex para remoção</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("pattern/{pattern}")]
        public async Task<IActionResult> RemoveCacheByPattern(string pattern)
        {
            await _cacheService.RemoveByPatternAsync(pattern);
            return Ok(new { message = $"Cache items matching pattern '{pattern}' removed successfully" });
        }

        /// <summary>
        /// Limpa todo o cache de produtos
        /// </summary>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("produtos")]
        public async Task<IActionResult> ClearProductCache()
        {
            await _cacheService.RemoveByPatternAsync(".*Produto.*");
            return Ok(new { message = "Product cache cleared successfully" });
        }

        /// <summary>
        /// Limpa todo o cache de clientes
        /// </summary>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("clientes")]
        public async Task<IActionResult> ClearClientCache()
        {
            await _cacheService.RemoveByPatternAsync(".*Cliente.*");
            return Ok(new { message = "Client cache cleared successfully" });
        }
    }
}