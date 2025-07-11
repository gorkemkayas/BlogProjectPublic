using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int categoryId)
            : base($"Category with ID {categoryId} not found.")
        {
        }
    }
}
