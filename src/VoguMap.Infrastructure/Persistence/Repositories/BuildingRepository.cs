using Microsoft.EntityFrameworkCore;
using VoguMap.Domain.Entities;
using VoguMap.Domain.Exceptions.Infrastructure;
using VoguMap.Domain.Interfaces.Repositories;
using VoguMap.Infrastructure.Persistence.Context;

namespace VoguMap.Infrastructure.Persistence.Repositories
{
    /// <inheritdoc/>
    public class BuildingRepository : IBuildingRepository
    {
        private readonly VoguMapContext _context;

        public BuildingRepository(
            VoguMapContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Building> CreateAsync(Building entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.Buildings
                .AddAsync(entity, token);

            await _context.SaveChangesAsync(token);
            return await GetByIdAsync(entity.Id, token)
                ?? throw new DataConsistencyException(
                    $"Building with ID {entity.Id} was created, but could not be retrieved.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Building entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.Buildings.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                return false;

            return await _context.Buildings
                .AnyAsync(b => b.Id == id, token);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Building>> GetAllAsync(CancellationToken token = default)
        {
            var entities = await _context.Buildings
                .AsNoTracking()
                .ToListAsync(token);

            return entities;
        }

        /// <inheritdoc/>
        public async Task<Building?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _context.Buildings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, token);

            return entity; 
        }

        /// <inheritdoc/>
        public async Task<Building?> GetByIdForUpdateAsync(int id, CancellationToken token = default)
        {
            var entity = await _context.Buildings
                .FirstOrDefaultAsync(b => b.Id == id, token);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<Building> UpdateAsync(Building entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.Buildings.Update(entity);
            await _context.SaveChangesAsync(token);

            return await GetByIdAsync(entity.Id, token)
                ?? throw new DataConsistencyException(
                    $"Building with ID {entity.Id} was updated, but could not be retrieved.");
        }
    }
}