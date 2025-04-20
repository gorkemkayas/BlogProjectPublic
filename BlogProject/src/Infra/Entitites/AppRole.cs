using Microsoft.AspNetCore.Identity;

namespace BlogProject.src.Infra.Entitites
{
    public class AppRole : IdentityRole<Guid>
    {
        public string? CreatedBy { get; set; }
    }
}
