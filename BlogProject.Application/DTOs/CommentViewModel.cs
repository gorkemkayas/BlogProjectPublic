using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid? ParentCommentId { get; set; }

        public AuthorDto Author { get; set; }

        public int LikeCount { get; set; }

        public List<CommentViewModel> Replies { get; set; } = new();
    }
}
