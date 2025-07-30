using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;

namespace BlogProject.Web.ViewModels
{
    public class TagViewModel
    {
        public TagEntity Tag { get; set; }
        public List<PostListItemDto> Posts { get; set; }
        public List<TagEntity> RelatedTags { get; set; }
        public int? TotalViews { get; set; }
        public int AuthorCount { get; set; }
        // Pagination için
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
