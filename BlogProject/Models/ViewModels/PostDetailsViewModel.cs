using BlogProject.Services.CustomMethods.Concrete;
using BlogProject.src.Infra.Entitites;

namespace BlogProject.Models.ViewModels
{
    internal class PostDetailsViewModel
    {
        public PostEntity Post { get; set; } = null!;
        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public ICollection<PostEntity> RecommendedPosts { get; set; } = new List<PostEntity>();

        public TimeSpan ReadingTime
        {
            get
            {
                return ReadingTimeCalculator.CalculateReadingTime(Post.Content);
            }
        }
    }
}