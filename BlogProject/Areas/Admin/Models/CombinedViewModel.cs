namespace BlogProject.Areas.Admin.Models
{
    public class CombinedViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> MostContributors { get; set; }
        public List<UserViewModel> NewMembers { get; set; }
    }
}
