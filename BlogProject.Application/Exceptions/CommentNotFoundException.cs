using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int commentId)
            : base($"Comment with ID {commentId} not found.")
        {
        }
    }
}
