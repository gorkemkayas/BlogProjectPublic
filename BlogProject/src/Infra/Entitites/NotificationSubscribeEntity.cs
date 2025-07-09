using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class NotificationSubscribeEntity : BaseEntity
    {
        public string SubscriberId { get; set; } = null!;
        public bool IsActive = true;
    }
}
