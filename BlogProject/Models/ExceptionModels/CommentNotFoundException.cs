using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int commentId)
            : base($"Comment with ID {commentId} not found.")
        {
        }
    }
}
