using System.ComponentModel.DataAnnotations;

namespace BlogProject.Application.DTOs
{
    public class SuspendUserDto
    {
        public string UserId { get; set; }

        [Required]
        public int SuspensionMinutes { get; set; }

        [Required]
        public string ReasonCategory { get; set; }

        [Required]
        public string ReasonDetail { get; set; }
    }
}
