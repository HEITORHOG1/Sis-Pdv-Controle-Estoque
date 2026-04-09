using Commands;
using MediatR;
using Interfaces;

namespace Sis_Pdv_Controle_Estoque_API.Behaviors
{
    /// <summary>
    /// Pipeline behavior that calls SaveChangesAsync after successful command execution.
    /// Runs after ValidationBehavior and the handler. If the handler returns Success=true
    /// (no notifications), persists all pending changes atomically.
    ///
    /// This replaces the previous pattern where SaveChanges was called in ControllerBase,
    /// ensuring proper transaction boundaries at the handler level.
    /// </summary>
    public class SaveChangesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveChangesBehavior<TRequest, TResponse>> _logger;

        public SaveChangesBehavior(IUnitOfWork unitOfWork, ILogger<SaveChangesBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            // Only persist if handler succeeded (no notifications/errors)
            if (response.Success)
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to persist changes for {RequestType}", typeof(TRequest).Name);
                    throw;
                }
            }

            return response;
        }
    }
}
