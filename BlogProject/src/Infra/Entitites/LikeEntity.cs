using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class LikeEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public Guid? PostId { get; set; }
        public PostEntity? Post { get; set; }

        public Guid? CommentId { get; set; }
        public CommentEntity? Comment { get; set; }
    }
}
