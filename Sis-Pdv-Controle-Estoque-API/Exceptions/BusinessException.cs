namespace Sis_Pdv_Controle_Estoque_API.Exceptions
{
    /// <summary>
    /// Base class for all business logic exceptions
    /// </summary>
    public abstract class BusinessException : Exception
    {
        protected BusinessException(string message) : base(message) { }
        
        protected BusinessException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class ValidationException : BusinessException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new[] { message };
        }

        public ValidationException(IEnumerable<string> errors) 
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        public ValidationException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors;
        }
    }

    /// <summary>
    /// Exception thrown when a requested resource is not found
    /// </summary>
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entityName, object key) 
            : base($"{entityName} with key '{key}' was not found.") { }

        public NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when trying to create a duplicate resource
    /// </summary>
    public class DuplicateException : BusinessException
    {
        public DuplicateException(string message) : base(message) { }

        public DuplicateException(string entityName, string field, object value)
            : base($"{entityName} with {field} '{value}' already exists.") { }
    }

    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleException : BusinessException
    {
        public BusinessRuleException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when user is not authorized to perform an action
    /// </summary>
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message = "You are not authorized to perform this action.") 
            : base(message) { }
    }
}