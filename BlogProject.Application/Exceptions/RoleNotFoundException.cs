using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(int roleId)
            : base($"Role with ID {roleId} not found.")
        {
        }
    }
}
