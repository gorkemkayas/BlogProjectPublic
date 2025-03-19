using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class ExtendedProfileViewModel : VisitorProfileViewModel
    {
        public string Id { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }

        public string? SecurityStamp {  get; set; }
        public string? ConcurrencyStamp { get; set; }
    }
}
