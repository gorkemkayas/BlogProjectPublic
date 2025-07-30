using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Content { get; set; }
        public int LikeCount { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int ViewCount { get; set; }
    }
}
