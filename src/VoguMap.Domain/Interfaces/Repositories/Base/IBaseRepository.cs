namespace VoguMap.Domain.Interfaces.Repositories.Base
{
    /// <summary>
    /// Абстрактный интерфейс с методами добавления и удаления
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип получаемой/создаваемой сущности (например Room)
    /// </typeparam>
    /// <typeparam name="TKey">
    /// Идентификатор (GUID или int)
    /// </typeparam>
    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        /// <summary>
        /// Получение сущности по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Найденная сущность или null</returns>
        Task<TEntity?> GetByIdAsync(
            TKey id,
            CancellationToken token = default);

        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Найденная сущности или пустой список</returns>
        Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken token = default);

        /// <summary>
        /// Удаление сущности по ID
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        /// <param name="token">Токен отмены операции</param>
        Task DeleteAsync(
            TEntity entity,
            CancellationToken token = default);

        /// <summary>
        /// Проверка сущнствования сущности по её ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>True - если найдено, False - не найдено</returns>
        Task<bool> ExistsByIdAsync(
            TKey id,
            CancellationToken token = default);

        /// <summary>
        /// Создание сущности
        /// </summary>
        /// <param name="entity">Создаваемая сущность</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Созданная сущность</returns>
        Task<TEntity> CreateAsync(
            TEntity entity,
            CancellationToken token = default);
    }
}