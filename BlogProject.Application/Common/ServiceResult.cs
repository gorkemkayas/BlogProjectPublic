using Microsoft.AspNetCore.Identity;

namespace BlogProject.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public List<IdentityError>? Errors { get; set; }
        public T? Data { get; set; }
    }
}
