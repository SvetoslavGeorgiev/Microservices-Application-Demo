namespace CommandService.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class CommandCreateDto
    {
        [Required]
        public string HowTo { get; set; } = null!;

        [Required]
        public string CommandLine { get; set; } = null!;
    }
}
