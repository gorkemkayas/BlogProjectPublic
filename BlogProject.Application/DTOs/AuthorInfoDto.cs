using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class AuthorInfoDto
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? ProfilePicture { get; set; }

        public string? Title { get; set; }
        public string? CurrentPosition { get; set; }
        public string? UniversityName { get; set; }

        public string? XAddress { get; set; }
        public string? LinkedinAddress { get; set; }
        public string? GithubAddress { get; set; }
        public string? MediumAddress { get; set; }
        public string? YoutubeAddress { get; set; }
        public string? PersonalWebAddress { get; set; }
    }
}
