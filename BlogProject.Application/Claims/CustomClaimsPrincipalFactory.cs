using BlogProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Claims
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // 1. Veritabanındaki claim'leri çek
            var dbClaims = await UserManager.GetClaimsAsync(user);

            foreach (var claim in dbClaims)
            {
                // Aynı claim varsa tekrar ekleme
                if (!identity.HasClaim(c => c.Type == claim.Type))
                    identity.AddClaim(claim);
            }

            // 2. Extra olarak direkt entity'den de ekleyebilirsin (güvenlik için override gibi düşün)
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                identity.AddClaim(new Claim("ProfilePictureUrl", user.ProfilePicture));
            }

            return identity;
        }
    }
}
