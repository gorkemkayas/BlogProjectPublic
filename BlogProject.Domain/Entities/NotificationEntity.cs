using BlogProject.Domain.Entities.Base;
using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
