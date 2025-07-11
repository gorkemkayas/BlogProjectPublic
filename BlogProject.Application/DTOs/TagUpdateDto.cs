namespace BlogProject.Application.DTOs
{
    public class TagUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
    }
}