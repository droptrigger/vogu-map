namespace VoguMap.Domain.Exceptions.Domain
{
    /// <summary>
    /// Ошибки домена (бизнес-логика)
    /// HTTP: 400, 404, 409
    /// </summary>
    public abstract class DomainException : BackendException
    {
        public override int HttpStatusCode => 400;

        protected DomainException() { }
        protected DomainException(string? message) : base(message) { }
        protected DomainException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
