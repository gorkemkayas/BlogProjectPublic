using BlogProject.src.Infra.Entitites.Base;

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

    public enum NotificationType
    {
        FromUser = 0,
        FromPost = 1,
        FromSystem = 2,

    }
}
