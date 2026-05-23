using Microsoft.AspNetCore.Mvc;
using VoguMap.Application.DTOs.Building;
using VoguMap.Application.Services.Interfaces;

namespace VoguMap.Web.Controllers.v1
{
    [ApiController]
    [Route("api/v1/building")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(
            IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        /// <summary>
        /// Получение учебного корпуса по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденный корпус.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BuildingGetDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var entity = await _buildingService
                .GetByIdAsync(id, cancellationToken);
            return Ok(entity);
        }

        /// <summary>
        /// Получение всех учебных корпусов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Все учебные корпуса.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<BuildingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _buildingService.GetAllAsync(cancellationToken);
            return Ok(entities);
        }

        /// <summary>
        /// Создание нового учебного корпуса.
        /// </summary>
        /// <param name="createDto">Данные для создания учебного корпуса.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Созданный учебный корпус.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BuildingGetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] BuildingCreateDto createDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEntity = await _buildingService
                .CreateAsync(createDto, cancellationToken);
                
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        /// <summary>
        /// Обновление существующего учебного корпуса.
        /// </summary>
        /// <param name="updateDto">Данные для обновления учебного корпуса.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Обновленный учебный корпус.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(BuildingGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromBody] BuildingUpdateDto updateDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _buildingService.UpdateAsync(updateDto, cancellationToken);
            return Ok(updated);
        }

        /// <summary>
        /// Удаление учебного корпуса по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Результат удаления.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _buildingService.DeleteByIdAsync(id, cancellationToken);
            return Ok(new { success = true, message = "Building deleted successfully" });
        }
    }
}