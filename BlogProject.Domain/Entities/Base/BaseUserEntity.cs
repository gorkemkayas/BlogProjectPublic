namespace BlogProject.Domain.Entities.Base
{
    public class BaseUserEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Bio { get; set; }
        public string WorkingAt { get; set; }

        public string Country { get; set; }
        public DateTime BirthDate { get; set; }

        public bool IsDeleted { get; set; }

        public string? ProfilePicture { get; set; }
        public string? CoverImagePicture { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int NotificationCount { get; set; } = 0;
    }
}
