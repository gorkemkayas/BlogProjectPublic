using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} not found.")
        {
        }
    }
}
