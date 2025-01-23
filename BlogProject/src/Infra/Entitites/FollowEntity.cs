using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class FollowEntity
    {
        public Guid FollowerId { get; set; }
        public UserEntity Follower { get; set; }

        public Guid FollowingId { get; set; }
        public UserEntity Following { get; set; }

        public DateTime FollowDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
