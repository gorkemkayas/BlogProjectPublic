namespace BlogProject.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneNumberConfirmed { get; set; } = false;
        public bool TwoFactorEnabled { get; set; } = false;

        public string? PhoneNumber { get; set; }

        public DateTime? RegisteredDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        
        public DateTime? SuspendedTo { get; set; }

        public bool IsSuspended { get; set; }
    }
}
