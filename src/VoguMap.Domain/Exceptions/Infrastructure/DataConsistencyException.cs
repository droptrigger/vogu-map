namespace VoguMap.Domain.Exceptions.Infrastructure
{
    /// <summary>
    /// Ошибка согласованности данных (500)
    /// </summary>
    public class DataConsistencyException : InfrastructureException
    {
        public override string ErrorCode => "DATA_CONSISTENCY";

        public DataConsistencyException(string message, Exception? innerException = null)
            : base(message, innerException) { }
    }
}