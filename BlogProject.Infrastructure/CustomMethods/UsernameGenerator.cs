using BlogProject.Application.CustomMethods.Interfaces;

namespace BlogProject.Infrastructure.CustomMethods
{
    public class UsernameGenerator : IUsernameGenerator
    {
        public string GenerateUsernameByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is required");
            }
            var userName = email.Substring(0, email.IndexOf("@"));
            userName += new Random().Next(1000, 9999);
            return userName;
        }

    }
}
