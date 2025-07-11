namespace BlogProject.Application.DTOs
{
    public class CategoryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsDraft { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
    }
}
