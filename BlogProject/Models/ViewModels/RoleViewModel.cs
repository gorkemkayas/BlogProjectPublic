namespace BlogProject.Models.ViewModels
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; } = null!; // Id value of role creator.
    }
}
