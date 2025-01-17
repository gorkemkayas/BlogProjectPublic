namespace BlogProject.src.Infra.Entitites
{
    public class BadgeUserEntity
    {
       
        public Guid BadgeId { get; set; }
        public virtual BadgeEntity Badge { get; set; }

        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }


        public DateTime AwardDate { get; set; }
    }
}
