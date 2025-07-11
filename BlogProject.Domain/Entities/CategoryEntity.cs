using BlogProject.Domain.Entities.Base;

namespace BlogProject.Domain.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDraft { get; set; }
        public virtual ICollection<PostEntity>? Posts { get; set; }

        public string CreatedBy { get; set; }
        public string? EditedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }

    }
}
