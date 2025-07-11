using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(int postId)
            : base($"Post with ID {postId} not found.")
        {
        }
    }
}
