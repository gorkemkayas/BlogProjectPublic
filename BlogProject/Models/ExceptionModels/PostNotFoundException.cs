using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(int postId)
            : base($"Post with ID {postId} not found.")
        {
        }
    }
}
