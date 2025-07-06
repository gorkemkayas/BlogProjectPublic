using BlogProject.src.Infra.Entitites;

namespace BlogProject.Models.ViewModels
{
    public class IndexViewModel
    {
        public ICollection<PostEntity> MostViewedPosts { get; set; } = new List<PostEntity>();
        public ICollection<PostEntity> LatestPosts { get; set; } = new List<PostEntity>();
        public ICollection<TagEntity> PopularTags { get; set; } = new List<TagEntity>();
    }
}
