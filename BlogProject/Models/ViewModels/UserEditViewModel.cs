using BlogProject.src.Infra.Entitites;

namespace BlogProject.Models.ViewModels
{
    public class UserEditViewModel
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string? WorkingAt { get; set; }
        public string? PhoneNumber { get; set; }
        //public IFormFile? WorkingAtLogo { get; set; }

        public string? LivesIn { get; set; }
        public string? Country { get; set; }

        // yeni eklediklerim
        public string? CurrentPosition { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public string? XAddress { get; set; }
        public string? LinkedinAddress { get; set; }
        public string? GithubAddress { get; set; }
        public string? MediumAddress { get; set; }
        public string? YoutubeAddress { get; set; }
        public string? PersonalWebAddress { get; set; }

        public string? HighSchoolName { get; set; }
        public string? HighSchoolStartYear { get; set; }
        public string? HighSchoolGraduationYear { get; set; }

        public string? UniversityName { get; set; }
        public string? UniversityStartYear { get; set; }
        public string? UniversityGraduationYear { get; set; }



        // sonu
        public DateTime BirthDate { get; set; }
        //public IFormFile? ProfilePicture { get; set; }
        //public IFormFile? CoverImagePicture { get; set; }
    }
}
