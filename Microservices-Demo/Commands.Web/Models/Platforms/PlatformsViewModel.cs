namespace Commands.Web.Models.Platforms
{
    using System.ComponentModel.DataAnnotations;

    public class PlatformsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public string Cost { get; set; } = null!;
    }
}
