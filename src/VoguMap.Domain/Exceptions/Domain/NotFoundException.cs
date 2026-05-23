namespace VoguMap.Domain.Exceptions.Domain
{
    /// <summary>
    /// Сущность не найдена (404)
    /// </summary>
    public class NotFoundException : DomainException
    {
        public override string ErrorCode => "NOT_FOUND";
        public override int HttpStatusCode => 404;

        public string EntityName { get; }
        public object EntityId { get; }

        public NotFoundException(string entityName, object entityId)
            : base($"{entityName} with id '{entityId}' was not found")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public NotFoundException(string entityName, string fieldName, object fieldValue)
            : base($"{entityName} with {fieldName} '{fieldValue}' was not found")
        {
            EntityName = entityName;
            EntityId = fieldValue;
        }
    }

}