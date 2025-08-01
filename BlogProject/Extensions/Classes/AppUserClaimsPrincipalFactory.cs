using BlogProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BlogProject.Web.Extensions.Classes
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Sadece ProfilePicture claim'i ekle - roles zaten base'de ekleniyor
            identity.AddClaim(new Claim("ProfilePictureUrl", user.ProfilePicture ?? "/images/default-avatar.png"));

            return identity;
        }
    }
}
