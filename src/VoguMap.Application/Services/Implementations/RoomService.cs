using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VoguMap.Application.Common.Pagination;
using VoguMap.Application.DTOs.Room;
using VoguMap.Application.Helpers;
using VoguMap.Application.Services.Interfaces;
using VoguMap.Domain.Entities;
using VoguMap.Domain.Exceptions.Domain;
using VoguMap.Domain.Filters;
using VoguMap.Domain.Interfaces.Repositories;

namespace VoguMap.Application.Services.Implementations
{
    /// <inheritdoc/>
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBuildingRepository _buildingRepository;

        private readonly IMapper _mapper;

        public RoomService(
            IRoomRepository roomRepository,
            IBuildingRepository buildingRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<RoomGetDto> CreateAsync(RoomCreateDto createDto, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            await ValidateBuildingIdAsync(createDto.BuildingId, token);
            ValidateFloor(createDto.Floor);

            var room = new Room
            {
                Name = createDto.Name,
                Description = createDto.Description,
                BuildingId = createDto.BuildingId,
                Floor = createDto.Floor
            };

            var result = await _roomRepository.CreateAsync(room, token);
            return _mapper.Map<RoomGetDto>(result);
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _roomRepository.GetByIdForUpdateAsync(id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Room), id);

            await _roomRepository.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RoomGetDto>> GetAsync(RoomFilterDto filter, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(filter);
            await ValidateBuildingIdAsync(filter.BuildingId, token);

            if (string.IsNullOrWhiteSpace(filter.Name))
                throw new ValidationException("The field Name is required.");

            var domainFilter = new RoomFilter(filter.BuildingId, filter.Name, filter.Floor);

            var result = await _roomRepository.GetAsync(domainFilter, token);
            return _mapper.Map<IReadOnlyList<RoomGetDto>>(result);
        }

        /// <inheritdoc/>
        public async Task<RoomGetDto> GetByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _roomRepository.GetByIdAsync(id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Room), id);

            return _mapper.Map<RoomGetDto>(entity);
        }

        /// <inheritdoc/>
        public async Task<PagedResultDto<RoomGetDto>> GetPagedAsync(
            int? buildingId = null,
            int? floor = null,
            string? search = null,
            string? sortBy = "Name",
            bool descending = true,
            int page = 1,
            int pageSize = 20,
            CancellationToken token = default)
        {
            if (page < 1)
                throw new ArgumentException("The page number cannot be less than 1.");

            if (pageSize > 200)
                throw new ArgumentException("The page size should not exceed 200.");

            Expression<Func<Room, bool>>? predicate = null;

            if (buildingId.HasValue)
            {
                await ValidateBuildingIdAsync(buildingId.Value, token);
                predicate = r => r.BuildingId == buildingId.Value;
            }

            if (floor.HasValue)
            {
                ValidateFloor(floor.Value);
                Expression<Func<Room, bool>> floorPredicate =
                    r => r.Floor == floor.Value;

                predicate = predicate == null
                    ? floorPredicate
                    : PredicateCombiner.Combine(predicate, floorPredicate);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var escaped = StringEscaper.EscapeLikeString(search);

                Expression<Func<Room, bool>>? searchPredicate = d =>
                    EF.Functions.ILike(d.Name, $"%{escaped}%");

                predicate = predicate == null
                    ? searchPredicate
                    : PredicateCombiner.Combine(predicate, searchPredicate);
            }

            Expression<Func<Room, object>> orderBy = sortBy?.ToLower() switch
            {
                "name" => r => r.Name,
                _ => r => r.Name
            };

            var pagedNews = await _roomRepository.GetPagedAsync(
                predicate,
                orderBy,
                descending,
                page,
                pageSize,
                token);

            var itemsDto = _mapper.Map<List<RoomGetDto>>(pagedNews.Items);

            return new PagedResultDto<RoomGetDto>(
                itemsDto,
                pagedNews.TotalCount,
                pagedNews.Page,
                pagedNews.PageSize);
        }

        /// <inheritdoc/>
        public async Task<RoomGetDto> UpdateAsync(RoomUpdateDto updateDto, CancellationToken token = default)
        {
            if (updateDto.Id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _roomRepository.GetByIdForUpdateAsync(updateDto.Id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Room), updateDto.Id);

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                entity.Name = updateDto.Name;

            if (updateDto.BuildingId.HasValue)
            {
                await ValidateBuildingIdAsync(updateDto.BuildingId.Value, token);
                entity.BuildingId = updateDto.BuildingId.Value;
            }

            if (updateDto.Floor.HasValue)
            {
                ValidateFloor(updateDto.Floor.Value);
                entity.Floor = updateDto.Floor.Value;
            }

            entity.Description = updateDto.Description;

            var result = await _roomRepository.UpdateAsync(entity, token);

            return _mapper.Map<RoomGetDto>(result);
        }

        #region Private

        /// <summary>
        /// Проверяет существует ли учебный корпус с заданным ID.
        /// Если не найден, вызывает исключение <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="buildingId">ID.</param>
        /// <param name="token">Токен отмены операции.</param>
        private async Task ValidateBuildingIdAsync(
            int buildingId,
            CancellationToken token = default)
        {
            var exists = await _buildingRepository.ExistsByIdAsync(buildingId, token);

            if (!exists)
                throw new NotFoundException(nameof(Building), buildingId);
        }

        /// <summary>
        /// Проверяет валидность значения этажа.
        /// Если значение не валидно, вызывает исключение <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="floor">Этаж.</param>
        private void ValidateFloor(int floor)
        {
            if (floor < -2 || floor > 5)
                throw new ValidationException("The floor must be between -2 and 5");
        }

        #endregion
    }
}