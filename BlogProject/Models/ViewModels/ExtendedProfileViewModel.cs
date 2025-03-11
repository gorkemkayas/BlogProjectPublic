namespace BlogProject.Models.ViewModels
{
    public class ExtendedProfileViewModel : VisitorProfileViewModel
    {
        public string Id { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
