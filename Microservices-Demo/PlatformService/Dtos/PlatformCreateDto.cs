namespace PlatformService.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class PlatformCreateDto
    {

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Publisher { get; set; } = null!;

        [Required]
        public string Cost { get; set; } = null!;
    }
}
