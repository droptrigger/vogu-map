namespace VoguMap.Application.Common.Pagination
{
    public class PagedResultDto<TEntity>
    {
        /// <summary>
        /// Массив элементов текущей страницы.
        /// </summary>
        public IReadOnlyList<TEntity> Items { get; init; } = Array.Empty<TEntity>();

        /// <summary>
        /// Общее количество записей во всей выборке.
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
        /// Общее количество страниц. Вычисляется автоматически.
        /// </summary>
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        /// <summary>
        /// Признак наличия следующей страницы.
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Признак наличия предыдущей страницы.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Пустой конструктор.
        /// <remarks>Обязателен для корректной работы <c>System.Text.Json</c>, <c>AutoMapper</c>, <c>Mapster</c> и других инструментов сериализации/маппинга.</remarks>
        /// </summary>
        public PagedResultDto() { }

        /// <summary>
        /// Инициализирует новый экземпляр DTO-обёртки пагинации.
        /// </summary>
        /// <param name="items">Элементы текущей страницы. Не может быть <c>null</c>.</param>
        /// <param name="totalCount">Общее количество записей в выборке.</param>
        /// <param name="page">Текущая страница (должно быть ≥ 1).</param>
        /// <param name="pageSize">Размер страницы (должен быть ≥ 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается при некорректных значениях пагинации.
        /// </exception>
        public PagedResultDto(IReadOnlyList<TEntity> items, int totalCount, int page, int pageSize)
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