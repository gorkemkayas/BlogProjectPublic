using BlogProject.Application.Exceptions.BaseExceptions;

namespace BlogProject.Application.Exceptions
{
    public sealed class PermissionNotFoundException : NotFoundException
    {
        public PermissionNotFoundException(int permissionId)
            : base($"Permission with ID {permissionId} not found.")
        {
        }
    }
}
