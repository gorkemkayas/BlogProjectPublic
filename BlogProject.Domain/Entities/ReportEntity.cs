using BlogProject.Domain.Entities.Base;
using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class ReportEntity : BaseEntity
    {
        public Reason Reason { get; set; }
        public Status Status { get; set; }

        public Guid? ReporterId { get; set; }
        public virtual AppUser? ReporterUser { get; set; }

        public Guid? ReporteduserId { get; set; }
        public virtual AppUser? ReportedUser { get; set; }

        public Guid? ReportedPostId { get; set; }
        public virtual PostEntity? ReportedPost { get; set; }
    }
}
