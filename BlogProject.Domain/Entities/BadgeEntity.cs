using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class BadgeEntity
    {
        public Guid Id { get; set; }
        public BadgeType BadgeType { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<BadgeUserEntity>? BadgeUsers { get; set; }

        public bool IsDeleted { get; set; }
    }
}
