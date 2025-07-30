using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace BlogProject.Web.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryIdNameDescriptionDto Category { get; set; }
        public List<PostListItemDto> Posts { get; set; }
        public List<PostWithCategoryNameDto> MostViewedPosts { get; set; }
        public List<PostDto> MostLikedPosts { get; set; }
        public List<CategoryIdAndNameDto> RelatedCategories { get; set; }
        public int TotalViews { get; set; }
        public int AuthorCount { get; set; }
        // Pagination için
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
