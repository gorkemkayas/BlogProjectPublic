
using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int categoryId)
            : base($"Category with ID {categoryId} not found.")
        {
        }
    }
}
