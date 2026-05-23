namespace VoguMap.Domain.Exceptions.Domain
{
    /// <summary>
    /// Ошибка валидации данных (400)
    /// </summary>
    public class ValidationException : DomainException
    {
        public override string ErrorCode => "VALIDATION_ERROR";

        public ValidationException(string? message = null)
            : base(message ?? "Validation failed")
        {
        }
    }
}