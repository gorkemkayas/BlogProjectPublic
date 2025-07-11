
using BlogProject.Domain.Entities.Base;
using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class SettingsEntity : BaseEntity
    {
        public Theme Theme { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
