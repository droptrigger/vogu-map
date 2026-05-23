using AutoMapper;
using VoguMap.Application.DTOs.Building;
using VoguMap.Application.Services.Interfaces;
using VoguMap.Domain.Entities;
using VoguMap.Domain.Exceptions.Domain;
using VoguMap.Domain.Interfaces.Repositories;
using ValidationException = VoguMap.Domain.Exceptions.Domain.ValidationException;

namespace VoguMap.Application.Services.Implementations
{
    /// <inheritdoc/>
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

        private readonly IMapper _mapper;

        public BuildingService(
            IBuildingRepository buildingRepository,
            IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<BuildingGetDto> CreateAsync(BuildingCreateDto createDto, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            var entity = new Building
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Longitude = createDto.Longitude,
                Latitude = createDto.Latitude
            };

            var created = await _buildingRepository.CreateAsync(entity, token);
            return _mapper.Map<BuildingGetDto>(created);
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _buildingRepository.GetByIdForUpdateAsync(id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Building), id);

            await _buildingRepository.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<BuildingGetDto>> GetAllAsync(CancellationToken token = default)
        {
            var entities = await _buildingRepository.GetAllAsync(token);
            return _mapper.Map<IReadOnlyList<BuildingGetDto>>(entities); 
        }

        /// <inheritdoc/>
        public async Task<BuildingGetDto> GetByIdAsync(int id, CancellationToken token = default)
        {
            if (id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _buildingRepository.GetByIdAsync(id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Building), id);

            return _mapper.Map<BuildingGetDto>(entity);
        }

        /// <inheritdoc/>
        public async Task<BuildingGetDto> UpdateAsync(BuildingUpdateDto updateDto, CancellationToken token = default)
        {
            if (updateDto.Id < 1)
                throw new ValidationException("The ID must not be less than 1.");

            var entity = await _buildingRepository.GetByIdForUpdateAsync(updateDto.Id, token);

            if (entity is null)
                throw new NotFoundException(nameof(Building), updateDto.Id);

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                entity.Name = updateDto.Name;

            if (!string.IsNullOrWhiteSpace(updateDto.Address))
                entity.Address = updateDto.Address;

            entity.Latitude = updateDto.Latitude;
            entity.Longitude = updateDto.Longitude;

            var result = await _buildingRepository.UpdateAsync(entity, token);

            return _mapper.Map<BuildingGetDto>(result);
        }
    }
}