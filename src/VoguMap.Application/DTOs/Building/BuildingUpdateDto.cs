using System.ComponentModel.DataAnnotations;

namespace VoguMap.Application.DTOs.Building
{
    public class BuildingUpdateDto
    {
        [Required(ErrorMessage = "The field Id is required.")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "The name should not exceed 100 characters in length.")]
        public string? Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "The address length should not exceed 200 characters.")]
        public string? Address { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "The Latitude must be between -90 and 90 degrees.")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The Longitude must be between -180 and 180 degrees.")]
        public decimal? Longitude { get; set; }
    }
}