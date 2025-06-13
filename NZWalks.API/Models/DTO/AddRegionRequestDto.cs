using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters")]
        public required string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage ="Name can not exceed 100 characters")]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
