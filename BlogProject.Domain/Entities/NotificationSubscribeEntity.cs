using BlogProject.Domain.Entities.Base;

namespace BlogProject.Domain.Entities
{
    public class NotificationSubscribeEntity : BaseEntity
    {
        public string SubscriberId { get; set; } = null!;
        public bool IsActive = true;
    }
}
