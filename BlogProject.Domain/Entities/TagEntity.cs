
using BlogProject.Domain.Entities.Base;

namespace BlogProject.Domain.Entities
{
    public class TagEntity : BaseEntity
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }

        public bool IsDraft { get; set; }

        public virtual ICollection<PostTagEntity>? TagPosts { get; set; }

        public string CreatedBy { get; set; }
        public string? EditedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
    }
}
