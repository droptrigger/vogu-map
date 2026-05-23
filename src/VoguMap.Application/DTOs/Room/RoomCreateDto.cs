using System.ComponentModel.DataAnnotations;

namespace VoguMap.Application.DTOs.Room
{
    public class RoomCreateDto
    {
        [Required(ErrorMessage = "The field BuildingId is required.")]
        public int BuildingId { get; set; }

        [Required(ErrorMessage = "The field Name is required.")]
        [StringLength(100, ErrorMessage = "The name should not exceed 100 characters in length.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Описание не может содержать больше 100 символов")]
        public string? Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The field Name is required.")]
        public int Floor { get; set; }
    }
}