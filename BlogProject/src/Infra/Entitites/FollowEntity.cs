using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class FollowEntity
    {
        public Guid FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public Guid FollowingId { get; set; }
        public AppUser Following { get; set; }

        public DateTime FollowDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
