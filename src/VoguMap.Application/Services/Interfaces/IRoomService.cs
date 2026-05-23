using VoguMap.Application.Common.Pagination;
using VoguMap.Application.DTOs.Room;
using VoguMap.Domain.Filters;

namespace VoguMap.Application.Services.Interfaces
{
    /// <summary>
    /// Сервис работы с помещениями.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Получает помещение по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Найденные данные о помещении.</returns>
        Task<RoomGetDto> GetByIdAsync(
            int id,
            CancellationToken token = default);

        /// <summary>
        /// Создает помещение и получает его.
        /// </summary>
        /// <param name="createDto">Данные для создания помещения.</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Созданное помещение.</returns>
        Task<RoomGetDto> CreateAsync(
            RoomCreateDto createDto,
            CancellationToken token = default);

        /// <summary>
        /// Удаляет помещение по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteByIdAsync(
            int id,
            CancellationToken token = default);

        /// <summary>
        /// Обновляет помещение, на основе предоставленных данных
        /// и получает его.
        /// </summary>
        /// <param name="updateDto">Данные для обновления.</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Обновленное помещение.</returns>
        Task<RoomGetDto> UpdateAsync(
            RoomUpdateDto updateDto,
            CancellationToken token = default);

        /// <summary>
        /// Получает список помещений по заданному фильтру.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Найденные помещения.</returns>
        Task<IReadOnlyList<RoomGetDto>> GetAsync(
            RoomFilter filter,
            CancellationToken token = default);

        /// <summary>
        /// Получает пагинированный список сущностей с фильтрацией и сортировкой.
        /// </summary>
        /// <param name="buildingId">ID корпуса.</param>
        /// <param name="floor">Этаж.</param>
        /// <param name="search">Поиск по имени.</param>
        /// <param name="sortBy">Сортировка по полю.</param>
        /// <param name="descending">Направление сортировки: <c>true</c> — по убыванию, <c>false</c> — по возрастанию.</param>
        /// <param name="page">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Количество записей на странице.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Пагинированный результат <see cref="PagedResultDto{Room}"/>.</returns>
        Task<PagedResultDto<RoomGetDto>> GetPagedAsync(
            int? buildingId = null,
            int? floor = null,
            string? search = null,
            string? sortBy = "Name",
            bool descending = true,
            int page = 1,
            int pageSize = 20,
            CancellationToken token = default);

    }
}