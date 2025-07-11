
using BlogProject.Domain.Entities;

namespace BlogProject.Web.ViewModels
{
    internal class PostDetailsViewModel
    {
        public PostEntity Post { get; set; } = null!;
        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public ICollection<PostEntity> RecommendedPosts { get; set; } = new List<PostEntity>();

        public AppUser? CurrentUser { get; set; }
        public TimeSpan ReadingTime
        {
            get
            {
                return ReadingTimeCalculator.CalculateReadingTime(Post.Content);
            }
        }
    }
}