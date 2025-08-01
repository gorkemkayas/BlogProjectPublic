using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class ContributorDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ProfileImageUrl { get; set; }
        public int PostCount { get; set; }
        public int FollowerCount { get; set; }
    }
}
