using BlogProject.Domain.Entities.Base;

namespace BlogProject.Domain.Entities
{
    public class LikeEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid? PostId { get; set; }
        public PostEntity? Post { get; set; }

        public Guid? CommentId { get; set; }
        public CommentEntity? Comment { get; set; }
    }
}
