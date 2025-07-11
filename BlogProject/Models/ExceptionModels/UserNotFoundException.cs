using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} not found.")
        {
        }
    }
}
