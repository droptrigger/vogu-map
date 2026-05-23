using Microsoft.AspNetCore.Mvc;
using VoguMap.Application.Common.Pagination;
using VoguMap.Application.DTOs.Room;
using VoguMap.Application.Services.Interfaces;

namespace VoguMap.Web.Controllers.v1
{
    [ApiController]
    [Route("api/v1/room")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController( 
            IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Получение помещения по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденное помещение.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomGetDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var entity = await _roomService
                .GetByIdAsync(id, cancellationToken);
            return Ok(entity);
        }

        /// <summary>
        /// Получение помещения по ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденное помещение.</returns>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] int buildingId,
            [FromQuery] string name,
            [FromQuery] int floor,
            CancellationToken cancellationToken)
        {
            var filter = new RoomFilterDto
            {
                BuildingId = buildingId,
                Name = name,
                Floor = floor
            };

            var entity = await _roomService
                .GetAsync(filter, cancellationToken);
            return Ok(entity);
        }

        /// <summary>
        /// Создание нового помещения.
        /// </summary>
        /// <param name="createDto">Данные для создания помещения.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Созданное помещение.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RoomGetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] RoomCreateDto createDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEntity = await _roomService
                .CreateAsync(createDto, cancellationToken);
                
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        /// <summary>
        /// Обновление существующего помещения.
        /// </summary>
        /// <param name="updateDto">Данные для обновления помещения.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Обновленное помещение.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(RoomGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromBody] RoomUpdateDto updateDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _roomService
                .UpdateAsync(updateDto, cancellationToken);
            
            return Ok(updated);
        }

        /// <summary>
        /// Удаление помещения.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Результат удаления.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _roomService.DeleteByIdAsync(id, cancellationToken);
            return Ok(new { success = true, message = "Building deleted successfully" });
        }

        /// <summary>
        /// Получение помещений с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы (по умолчанию 1).</param>
        /// <param name="pageSize">Размер страницы (по умолчанию 20, макс. 200).</param>
        /// <param name="search">Поиск по заголовку или контенту.</param>
        /// <param name="buildingId">ID учебного корпуса.</param>
        /// <param name="floor">Номер этажа</param>
        /// <param name="sortBy">Поле для сортировки: Name.</param>
        /// <param name="descending">Сортировка по убыванию.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<RoomGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] int? buildingId = null,
            [FromQuery] int? floor = null,
            [FromQuery] string sortBy = "Name",
            [FromQuery] bool descending = false,
            CancellationToken cancellationToken = default)
        {
            var result = await _roomService.GetPagedAsync(
                buildingId: buildingId,
                floor: floor,
                search: search,
                sortBy: sortBy,
                descending: descending,
                page: page,
                pageSize: pageSize,
                token: cancellationToken);

            return Ok(result);
        }
    }
}