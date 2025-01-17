using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class SettingsEntity : BaseEntity
    {
        public Theme Theme { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
