using VoguMap.Domain.Entities;
using VoguMap.Domain.Filters;
using VoguMap.Domain.Interfaces.Repositories.Base;

namespace VoguMap.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Репозиторий для работы с помещениями.
    /// </summary>
    public interface IRoomRepository :
        IBaseRepository<Room, int>,
        IBaseUpdateRepository<Room, int>,
        IBasePaginationRepository<Room>
    {
        /// <summary>
        /// Получает список помещений по заданным критериям фильтрации.
        /// </summary>
        /// <param name="filter">Параметры фильтрации помещений.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Список найденных помещений.</returns>
        Task<IReadOnlyList<Room>> GetAsync(
            RoomFilter filter,
            CancellationToken token = default);
    }
}