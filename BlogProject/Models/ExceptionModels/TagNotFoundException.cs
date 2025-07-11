using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(int tagId)
            : base($"Tag with ID {tagId} not found.")
        {
        }
    }
}
