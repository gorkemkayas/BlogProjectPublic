namespace BlogProject.Models.ViewModels
{
    public abstract class BaseProfileViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string FullName => $"{Name} {Surname}";
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? WorkingAt { get; set; }
        public string? Country { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? RegisteredDate { get; set; }

        public int FollowersCount { get; set; } 
        public int FollowingCount { get; set; }

        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverImagePicture { get; set; }
    }
}
