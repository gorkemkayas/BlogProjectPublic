using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.src.Infra.Entitites
{
    public class SubscriptionEntity
    {
        public DateTime SubscriptionDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public bool IsActive { get; set; }

        public Guid FollowerId { get; set; }
        public UserEntity Follower {  get; set; }

        public Guid FollowingId { get; set; }
        public UserEntity Following { get; set; }

        public bool IsDeleted { get; set; }

    }
}
