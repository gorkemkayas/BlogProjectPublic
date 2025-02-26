using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
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
