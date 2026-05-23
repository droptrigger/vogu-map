using System.Linq.Expressions;
using VoguMap.Domain.Common;

namespace VoguMap.Domain.Interfaces.Repositories.Base
{
    /// <summary>
    /// Абстрактный интерфейс с методом получения пагинированного
    /// списка сущностей
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущности для пагинации
    /// </typeparam>
    public interface IBasePaginationRepository<TEntity> 
        where TEntity : class
    {
        /// <summary>
        /// Получает пагинированный список сущностей с фильтрацией и сортировкой.
        /// </summary>
        /// <param name="predicate">Фильтр (например: <c>n => !n.IsArchived</c>). Если <c>null</c> — без фильтрации.</param>
        /// <param name="orderBy">Поле для сортировки (например: <c>n => n.CreatedAt</c>). Если <c>null</c> — сортировка по дате создания.</param>
        /// <param name="descending">Направление сортировки: <c>true</c> — по убыванию, <c>false</c> — по возрастанию.</param>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Количество записей на странице.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Пагинированный результат <see cref="PagedResult{TEntity}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="page"/> или <paramref name="pageSize"/> меньше 1.</exception>
        Task<PagedResult<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool descending = true,
            int page = 1,
            int pageSize = 20,
            CancellationToken token = default);

    }
}