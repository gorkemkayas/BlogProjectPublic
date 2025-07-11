using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(int tagId)
            : base($"Tag with ID {tagId} not found.")
        {
        }
    }
}
