namespace VoguMap.Domain.Exceptions.Infrastructure
{
    /// <summary>
    /// Технические ошибки (БД, сеть, конфигурация)
    /// HTTP: 500, 503
    /// </summary>
    public abstract class InfrastructureException : BackendException
    {
        public override int HttpStatusCode => 500;

        protected InfrastructureException() { }
        protected InfrastructureException(string? message) : base(message) { }
        protected InfrastructureException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}