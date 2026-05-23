using VoguMap.Application.DTOs.Building;

namespace VoguMap.Application.Services.Interfaces
{
    /// <summary>
    /// Сервис работы с учебными корпусами.
    /// </summary>
    public interface IBuildingService
    {
        /// <summary>
        /// Получает учебный корпус по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Найденные данные о учебном корпусе.</returns>
        Task<BuildingGetDto> GetByIdAsync(
            int id,
            CancellationToken token = default);

        /// <summary>
        /// Создает учебный корпус и получает его.
        /// </summary>
        /// <param name="createDto">Данные для создания учебного корпуса.</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Созданный учебный корпус.</returns>
        Task<BuildingGetDto> CreateAsync(
            BuildingCreateDto createDto,
            CancellationToken token = default);

        /// <summary>
        /// Удаляет учебный корпус по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteByIdAsync(
            int id,
            CancellationToken token = default);

        /// <summary>
        /// Обновляет учебный корпус, на основе предоставленных данных
        /// и получает его.
        /// </summary>
        /// <param name="updateDto">Данные для обновления.</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Обновленный учебный корпус.</returns>
        Task<BuildingGetDto> UpdateAsync(
            BuildingUpdateDto updateDto,
            CancellationToken token = default);

        /// <summary>
        /// Получает список всех учебных корпусов.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Список всех учебных корпусов.</returns>
        Task<IReadOnlyList<BuildingGetDto>> GetAllAsync(
            CancellationToken token = default);
    }
}