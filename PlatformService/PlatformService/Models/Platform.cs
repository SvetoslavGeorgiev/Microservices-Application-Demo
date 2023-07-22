namespace PlatformService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Platform
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Publisher { get; set; } = null!;

        [Required]
        public string Cost { get; set; } = null!;
    }
}
