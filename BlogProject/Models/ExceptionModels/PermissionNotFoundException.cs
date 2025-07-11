using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class PermissionNotFoundException : NotFoundException
    {
        public PermissionNotFoundException(int permissionId)
            : base($"Permission with ID {permissionId} not found.")
        {
        }
    }
}
