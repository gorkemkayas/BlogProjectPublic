using System.ComponentModel.DataAnnotations;

namespace BlogProject.Areas.Admin.Models
{
    public class SuspendUserViewModel
    {
        public string UserId { get; set; }

        [Required]
        public int SuspensionMinutes { get; set; }
    }
}
