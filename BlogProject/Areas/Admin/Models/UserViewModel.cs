namespace BlogProject.Areas.Admin.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? RegisteredDate { get; set; }
    }
}
