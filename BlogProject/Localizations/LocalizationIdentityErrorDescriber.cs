using Microsoft.AspNetCore.Identity;

namespace BlogProject.Localizations
{
    public class LocalizationIdentityErrorDescriber: IdentityErrorDescriber
    {
        // We can override the default messages here
        public override IdentityError InvalidEmail(string? email)
        {
            return base.InvalidEmail(email);    
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return base.DuplicateEmail(email);

        }
    }
}
