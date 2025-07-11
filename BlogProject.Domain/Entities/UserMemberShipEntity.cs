using BlogProject.Domain.Entities.Base;

namespace BlogProject.Domain.Entities
{
    public class UserMemberShipEntity : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public virtual ICollection<MemberShipTypeEntity> MembershipTypes { get; set; }
    }
}
