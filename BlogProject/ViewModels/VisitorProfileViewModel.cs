using BlogProject.Application.DTOs;
using BlogProject.Application.Validators;
using BlogProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Web.ViewModels
{
    public class VisitorProfileViewModel : BaseProfileViewModel
    {
        public string UserName { get; set; }
        public string? VisitedUserId { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? WorkingAt { get; set; }
        //public IFormFile? WorkingAtLogo { get; set; }
        public string? Country { get; set; }

        // yeni eklediklerim

        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }

        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public string? CurrentPosition { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        [HttpsValidator]
        public string? XAddress { get; set; }
        [HttpsValidator]
        public string? LinkedinAddress { get; set; }
        [HttpsValidator]
        public string? GithubAddress { get; set; }
        [HttpsValidator]
        public string? MediumAddress { get; set; }
        [HttpsValidator]
        public string? YoutubeAddress { get; set; }
        [HttpsValidator]
        public string? PersonalWebAddress { get; set; }

        public string? HighSchoolName { get; set; }
        public string? HighSchoolStartYear { get; set; }
        public string? HighSchoolGraduationYear { get; set; }

        public string? UniversityName { get; set; }
        public string? UniversityStartYear { get; set; }
        public string? UniversityGraduationYear { get; set; }

        // sonu
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public DateTime RegisteredDate { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverImagePicture { get; set; }

        public string? WorkingAtLogo { get; set; }
        public List<PostCardDto>? FeaturedPosts { get; set; }
        public List<PostCardDto>? RecentPosts { get; set; }

        public bool? IsFollowing { get; set; }
    }
}
