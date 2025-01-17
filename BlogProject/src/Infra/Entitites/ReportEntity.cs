using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class ReportEntity : BaseEntity
    {
        public  Reason Reason { get; set; }
        public Status Status { get; set; }

        public Guid? ReporterId { get; set; }
        public virtual UserEntity? ReporterUser { get; set; }

        public Guid? ReporteduserId { get; set; }
        public virtual UserEntity? ReportedUser { get; set; }

        public Guid? ReportedPostId { get; set; }
        public virtual PostEntity? ReportedPost { get; set; }
    }
}
