namespace BlogProject.Areas.Admin.Models
{
    public class CategoryUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
    }
}