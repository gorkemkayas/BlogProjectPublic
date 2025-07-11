using BlogProject.Models.ExceptionModels.Base;

namespace BlogProject.Models.ExceptionModels
{
    public sealed class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(int roleId)
            : base($"Role with ID {roleId} not found.")
        {
        }
    }
}
