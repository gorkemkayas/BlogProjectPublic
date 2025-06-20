using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.EntityTypeConfigurations;

namespace BlogProject.src.Infra.Entitites
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
