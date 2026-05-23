using System.ComponentModel.DataAnnotations;

namespace VoguMap.Application.DTOs.Room
{
    public class RoomUpdateDto
    {
        [Required(ErrorMessage = "The field Id is required.")]
        public int Id { get; set; }

        public int? BuildingId { get; set; } = null;

        [StringLength(100, ErrorMessage = "The name should not exceed 100 characters in length.")]
        public string? Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Описание не может содержать больше 100 символов")]
        public string? Description { get; set; } = string.Empty;

        public int? Floor { get; set; } = null;
    }
}