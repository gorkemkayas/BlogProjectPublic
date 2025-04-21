namespace BlogProject.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; } = null!; // Id value of role creator.
        public string? EditedBy { get; set; } // Id value of role editor.
    }
}
