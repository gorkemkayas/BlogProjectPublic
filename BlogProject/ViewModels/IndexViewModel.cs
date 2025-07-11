using BlogProject.Domain.Entities;

namespace BlogProject.Web.ViewModels
{
    public class IndexViewModel
    {
        public ICollection<PostEntity> MostViewedPosts { get; set; } = new List<PostEntity>();
        public ICollection<PostEntity> LatestPosts { get; set; } = new List<PostEntity>();
        public ICollection<TagEntity> PopularTags { get; set; } = new List<TagEntity>();
        public ICollection<AppUser> MostContributors { get; set; } = new List<AppUser>();
        public ICollection<PostEntity> MostReadPostsThisWeek { get; set; } = new List<PostEntity>();
    }
}
