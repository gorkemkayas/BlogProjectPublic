namespace BlogProject.Areas.Admin.Models
{
    public class ManageUserRolesViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<UserRoleViewModel> Roles { get; set; } = new List<UserRoleViewModel>();
    }
}
