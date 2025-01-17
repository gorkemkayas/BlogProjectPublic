using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class CommentEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Guid PostId { get; set; }
        public virtual PostEntity Post { get; set; }


        // ParentComment yoksa null olabilir.
        public Guid? ParentCommentId { get; set; }
        public virtual CommentEntity? ParentComment { get; set; }

        public Guid AuthorId { get; set; }
        public virtual UserEntity Author { get; set; }

        public virtual ICollection<CommentEntity>? Replies { get; set; }
        public virtual ICollection<LikeEntity>? Likes { get; set; }
    }
}
