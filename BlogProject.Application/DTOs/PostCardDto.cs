using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class PostCardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string? CoverImageUrl { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
