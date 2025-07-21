using BlogProject.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace BlogProject.Web.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryEntity Category { get; set; }
        public List<PostEntity> Posts { get; set; }
        public List<PostEntity> MostViewedPosts { get; set; }
        public List<PostEntity> MostLikedPosts { get; set; }
        public List<CategoryEntity> RelatedCategories { get; set; }
        public int TotalViews { get; set; }
        public int AuthorCount { get; set; }
        // Pagination için
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
