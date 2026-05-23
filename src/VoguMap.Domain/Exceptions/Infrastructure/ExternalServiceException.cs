namespace VoguMap.Domain.Exceptions.Infrastructure
{
    /// <summary>
    /// Ошибка внешнего сервиса (503)
    /// </summary>
    public class ExternalServiceException : InfrastructureException
    {
        public override string ErrorCode => "EXTERNAL_SERVICE";
        public override int HttpStatusCode => 503;

        public string ServiceName { get; }

        public ExternalServiceException(string serviceName, string message, Exception? innerException = null)
            : base($"Service '{serviceName}' failed: {message}", innerException)
        {
            ServiceName = serviceName;
        }

    }
}