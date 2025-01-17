using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class NotificationEntity : BaseEntity
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
