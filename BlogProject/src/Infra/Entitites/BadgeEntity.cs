using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class BadgeEntity
    {
        public Guid Id { get; set; }
        public BadgeType BadgeType { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<BadgeUserEntity>? BadgeUsers { get; set; }
    }
}
