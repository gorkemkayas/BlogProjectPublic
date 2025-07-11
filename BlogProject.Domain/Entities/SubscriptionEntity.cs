using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Domain.Entities
{
    public class SubscriptionEntity
    {
        public DateTime SubscriptionDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public bool IsActive { get; set; }

        public Guid FollowerId { get; set; }
        public AppUser Follower {  get; set; }

        public Guid FollowingId { get; set; }
        public AppUser Following { get; set; }

        public bool IsDeleted { get; set; }

    }
}
