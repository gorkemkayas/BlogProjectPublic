namespace BlogProject.Areas.Admin.Models
{
    public class CategoryAddViewModel
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string? EditedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? EditedDate { get; set; }
    }
}