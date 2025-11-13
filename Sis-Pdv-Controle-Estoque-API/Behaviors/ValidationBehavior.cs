using FluentValidation;
using MediatR;
using Commands;

namespace Sis_Pdv_Controle_Estoque_API.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation failed for {RequestType}. Errors: {Errors}",
                    typeof(TRequest).Name,
                    string.Join(", ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}")));

                // Create a response with validation errors
                var response = Activator.CreateInstance<TResponse>();
                
                // Use reflection to set the validation errors
                var responseType = typeof(TResponse);
                var notificationsProperty = responseType.GetProperty("Notifications");
                var successProperty = responseType.GetProperty("Success");

                if (notificationsProperty != null && successProperty != null)
                {
                    var notifications = failures.Select(f => new prmToolkit.NotificationPattern.Notification(f.PropertyName, f.ErrorMessage));
                    notificationsProperty.SetValue(response, notifications);
                    successProperty.SetValue(response, false);
                }

                return response;
            }

            return await next();
        }
    }
}