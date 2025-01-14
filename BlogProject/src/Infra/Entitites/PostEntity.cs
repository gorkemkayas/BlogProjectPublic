using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.EntityTypeConfigurations;

namespace BlogProject.src.Infra.Entitites
{
    public class PostEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string Content { get; set; } = null!;
        public string? SubContent { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool isDraft { get; set; } = false;
        public int ViewCount { get; set; }

        public Guid AuthorId { get; set; }
        public UserEntity Author { get; set; }

        public Guid CommentId { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
