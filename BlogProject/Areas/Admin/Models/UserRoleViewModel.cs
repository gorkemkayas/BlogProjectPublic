namespace BlogProject.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Exists { get; set; }
    }
}
