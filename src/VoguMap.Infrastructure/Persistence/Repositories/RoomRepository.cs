using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VoguMap.Domain.Common;
using VoguMap.Domain.Entities;
using VoguMap.Domain.Exceptions.Infrastructure;
using VoguMap.Domain.Filters;
using VoguMap.Domain.Interfaces.Repositories;
using VoguMap.Infrastructure.Persistence.Context;

namespace VoguMap.Infrastructure.Persistence.Repositories
{
    /// <inheritdoc/>
    public class RoomRepository : IRoomRepository
    {
        private readonly VoguMapContext _context;

        public RoomRepository(
            VoguMapContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Room> CreateAsync(Room entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.Rooms
                .AddAsync(entity, token);

            await _context.SaveChangesAsync(token);
            return await GetByIdAsync(entity.Id, token)
                ?? throw new DataConsistencyException(
                    $"Room with ID {entity.Id} was created, but could not be retrieved.");
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Room entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.Rooms.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                return false;

            return await _context.Rooms
                .AnyAsync(r => r.Id == id, token);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken token = default)
        {
            var entities = await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Building)
                .ToListAsync(token);

            return entities;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Room>> GetAsync(RoomFilter filter, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            var query = _context.Rooms
                .AsQueryable()
                .AsNoTracking();

            query = query.Where(
                r => r.BuildingId == filter.BuildingId &&
                string.Equals(r.Name.ToLower(), filter.Name.ToLower()));

            if (filter.Floor.HasValue)
                query = query.Where(r => r.Floor == filter.Floor.Value);

            return await query
                .Include(r => r.Building)
                .ToListAsync(token);
        }

        /// <inheritdoc/>
        public async Task<Room?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Building)
                .FirstOrDefaultAsync(r => r.Id == id, token);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<Room?> GetByIdForUpdateAsync(int id, CancellationToken token = default)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id, token);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Room>> GetPagedAsync(
            Expression<Func<Room, bool>>? predicate = null,
            Expression<Func<Room, object>>? orderBy = null,
            bool descending = true,
            int page = 1,
            int pageSize = 20,
            CancellationToken token = default)
        {
            var query = _context.Rooms
                .AsNoTracking()
                .AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalCount = await query.CountAsync(token);

            if (totalCount == 0)
            {
                return PagedResult<Room>.Empty(page, pageSize);
            }

            if (orderBy != null)
            {
                query = descending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(g => g.Name);
            }

            var items = await query
                .Include(r => r.Building)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            return new PagedResult<Room>(items, totalCount, page, pageSize);
        }

        /// <inheritdoc/>
        public async Task<Room> UpdateAsync(Room entity, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.Rooms.Update(entity);
            await _context.SaveChangesAsync(token);

            return await GetByIdAsync(entity.Id, token)
                ?? throw new DataConsistencyException(
                    $"Room with ID {entity.Id} was updated, but could not be retrieved.");
        }
    }
}