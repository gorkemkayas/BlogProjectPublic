namespace BlogProject.Services.CustomMethods.Abstract
{
    public interface IUsernameGenerator
    {
        string GenerateUsernameByEmail(string email);
    }
}
