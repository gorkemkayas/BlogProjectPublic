using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class PostDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public string CategoryName { get; set; } = null!;
        public AuthorInfoDto Author { get; set; } = null!;
    }
}
