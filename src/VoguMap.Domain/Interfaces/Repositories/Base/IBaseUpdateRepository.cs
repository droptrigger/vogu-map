namespace VoguMap.Domain.Interfaces.Repositories.Base
{
    /// <summary>
    /// Абстрактный интерфейс репозитория с методам обновления сущности
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип обновляемой сущности (например Room)
    /// </typeparam>
    /// <typeparam name="TKey">
    /// Идентификатор (GUID или int)
    /// </typeparam>
    public interface IBaseUpdateRepository<TEntity, TKey>
    {
        /// <summary>
        /// Получение сущности со свойством Tracked (отслеживаемой)
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Найденная сущность (отслеживаемая) или null</returns>
        Task<TEntity?> GetByIdForUpdateAsync(
            TKey id, 
            CancellationToken token = default);

        /// <summary>
        /// Обновление сущности
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Обновленная сущность</returns>
        Task<TEntity> UpdateAsync(
            TEntity entity, 
            CancellationToken token = default);
    }
}