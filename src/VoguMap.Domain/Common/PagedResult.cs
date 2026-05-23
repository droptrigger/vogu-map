namespace VoguMap.Domain.Common
{
    /// <summary>
    /// Универсальная обёртка для пагинированного результата.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущности для пагинации
    /// </typeparam>
    public class PagedResult<TEntity>
    {
        /// <summary>
        /// Коллекция элементов текущей страницы.
        /// </summary>
        public IReadOnlyList<TEntity> Items { get; init; } = [];

        /// <summary>
        /// Общее количество записей во всей выборке (без учёта пагинации).
        /// Используется для расчёта общего числа страниц на UI.
        /// </summary>
        public int TotalCount { get; init; }

        /// <summary>
        /// Номер текущей страницы (начиная с 1).
        /// </summary>
        public int Page { get; init; }

        /// <summary>
        /// Количество элементов на одной странице.
        /// </summary>
        public int PageSize { get; init; }

        /// <summary>
        /// Общее количество страниц. Вычисляется автоматически на основе <see cref="TotalCount"/> и <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        /// <summary>
        /// Возвращает <c>true</c>, если существует следующая страница.
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Возвращает <c>true</c>, если существует предыдущая страница.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Инициализирует новый экземпляр результата пагинации.
        /// </summary>
        /// <param name="items">Элементы текущей страницы. Не может быть <c>null</c>.</param>
        /// <param name="totalCount">Общее количество записей в выборке.</param>
        /// <param name="page">Текущая страница (должно быть ≥ 1).</param>
        /// <param name="pageSize">Размер страницы (должен быть ≥ 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="page"/> &lt; 1 или <paramref name="pageSize"/> &lt; 1.
        /// </exception>
        public PagedResult(IReadOnlyList<TEntity> items, int totalCount, int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Номер страницы должен быть больше или равен 1.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше или равен 1.");

            Items = items ?? Array.Empty<TEntity>();
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

    }
}