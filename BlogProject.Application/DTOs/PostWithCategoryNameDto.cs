using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class PostWithCategoryNameDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public int ViewCount { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CoverImageUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
