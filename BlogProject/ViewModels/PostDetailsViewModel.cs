
using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.CustomMethods;

namespace BlogProject.Web.ViewModels
{
    public class PostDetailsViewModel
    {
        public PostDetailsDto Post { get; set; } = null!;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public ICollection<PostDto> RecommendedPosts { get; set; } = new List<PostDto>();
        public bool IsLikedFromCurrentUser { get; set; } = false;
        public CurrentUserDto? CurrentUser { get; set; }
        public TimeSpan ReadingTime
        {
            get
            {
                return ReadingTimeCalculator.CalculateReadingTime(Post.Content);
            }
        }
    }
}