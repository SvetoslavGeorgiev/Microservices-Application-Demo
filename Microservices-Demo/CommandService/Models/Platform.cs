namespace CommandService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Platform
    {

        public Platform()
        {
            Commands = new HashSet<Command>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Command> Commands { get; set; }


    }
}
