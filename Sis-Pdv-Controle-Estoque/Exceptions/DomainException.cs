namespace Model.Exceptions
{
    /// <summary>
    /// Exceção lançada quando regras de negócio do domínio são violadas
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
