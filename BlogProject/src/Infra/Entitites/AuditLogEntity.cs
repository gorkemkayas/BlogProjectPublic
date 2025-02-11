using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class AuditLogEntity : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string Operation { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

    }
}
