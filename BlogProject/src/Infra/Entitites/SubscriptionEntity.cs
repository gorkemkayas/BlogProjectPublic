namespace BlogProject.src.Infra.Entitites
{
    public class SubscriptionEntity
    {
        public DateTime SubscriptionDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public bool IsActive { get; set; }

        public UserEntity FollowerId { get; set; }
        public UserEntity Follower {  get; set; }

        public UserEntity FollowingId { get; set; }
        public UserEntity Following { get; set; }

    }
}
