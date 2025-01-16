using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class SettingsEntity : BaseEntity
    {
        public Theme Theme { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }

    public enum Theme
    {
        Light = 0,
        Dark = 1,
        System = 2
    }
    public enum NotificationPreferences
    {
        OnlyUser = 0,
        OnlyPost = 1,
        All = 2
    }
}
