using Microsoft.AspNetCore.Identity;

namespace BlogProject.Utilities
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public List<IdentityError>? Errors { get; set; }
        public T? Data { get; set; }
    }
}
