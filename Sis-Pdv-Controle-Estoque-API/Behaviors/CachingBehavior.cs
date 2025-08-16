using MediatR;
using Sis_Pdv_Controle_Estoque_API.Services.Cache;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_API.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(ICacheService cacheService, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Only cache queries (requests that don't modify data)
            if (!IsQueryRequest(request))
            {
                return await next();
            }

            var cacheKey = GenerateCacheKey(request);
            
            try
            {
                // Try to get from cache first
                var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey);
                if (cachedResponse != null)
                {
                    _logger.LogDebug("Cache hit for request: {RequestType} with key: {CacheKey}", 
                        typeof(TRequest).Name, cacheKey);
                    return cachedResponse;
                }

                // Execute the request
                var response = await next();

                // Cache the response if it's successful
                if (response != null && IsSuccessfulResponse(response))
                {
                    var expiration = GetCacheExpiration(request);
                    await _cacheService.SetAsync(cacheKey, response, expiration);
                    
                    _logger.LogDebug("Cached response for request: {RequestType} with key: {CacheKey} for {Expiration}", 
                        typeof(TRequest).Name, cacheKey, expiration);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in caching behavior for request: {RequestType}", typeof(TRequest).Name);
                // If caching fails, still execute the request
                return await next();
            }
        }

        private static bool IsQueryRequest(TRequest request)
        {
            var requestType = request.GetType();
            var requestName = requestType.Name;

            // Consider requests as queries if they contain these keywords
            var queryKeywords = new[] { "Listar", "Obter", "Buscar", "Consultar", "Get", "List", "Find", "Search" };
            
            return queryKeywords.Any(keyword => requestName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsSuccessfulResponse(TResponse response)
        {
            // Check if response indicates success
            if (response == null) return false;

            // If response has a Success property, check it
            var successProperty = response.GetType().GetProperty("Success");
            if (successProperty != null && successProperty.PropertyType == typeof(bool))
            {
                return (bool)successProperty.GetValue(response)!;
            }

            // If response has Notifications property (from prmToolkit), check if empty
            var notificationsProperty = response.GetType().GetProperty("Notifications");
            if (notificationsProperty != null)
            {
                var notifications = notificationsProperty.GetValue(response);
                if (notifications is System.Collections.ICollection collection)
                {
                    return collection.Count == 0;
                }
            }

            // Default to true if we can't determine success status
            return true;
        }

        private static string GenerateCacheKey(TRequest request)
        {
            var requestType = request.GetType().Name;
            var requestData = JsonSerializer.Serialize(request);
            var hash = requestData.GetHashCode();
            
            return $"{requestType}:{hash}";
        }

        private static TimeSpan GetCacheExpiration(TRequest request)
        {
            var requestType = request.GetType();
            var requestName = requestType.Name;

            // Different cache durations based on request type
            return requestName switch
            {
                var name when name.Contains("Produto") => TimeSpan.FromMinutes(15), // Products change frequently
                var name when name.Contains("Cliente") => TimeSpan.FromMinutes(30), // Customers change less frequently
                var name when name.Contains("Fornecedor") => TimeSpan.FromHours(1), // Suppliers change rarely
                var name when name.Contains("Categoria") => TimeSpan.FromHours(2), // Categories change very rarely
                var name when name.Contains("Departamento") => TimeSpan.FromHours(2), // Departments change very rarely
                _ => TimeSpan.FromMinutes(30) // Default cache duration
            };
        }
    }
}