namespace VoguMap.Domain.Exceptions
{
    public abstract class BackendException : Exception
    {
        public abstract string ErrorCode { get; }
        public abstract int HttpStatusCode { get; }

        protected BackendException() { }
        protected BackendException(string? message) : base(message) { }
        protected BackendException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}