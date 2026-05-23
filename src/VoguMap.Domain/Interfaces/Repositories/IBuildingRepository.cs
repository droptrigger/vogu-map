using VoguMap.Domain.Entities;
using VoguMap.Domain.Interfaces.Repositories.Base;

namespace VoguMap.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Репозиторий для работы с учебными корпусами.
    /// </summary>
    public interface IBuildingRepository :
        IBaseRepository<Building, int>,
        IBaseUpdateRepository<Building, int>
    {

    }
}