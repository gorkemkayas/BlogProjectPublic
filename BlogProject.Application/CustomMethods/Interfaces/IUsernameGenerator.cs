namespace BlogProject.Application.CustomMethods.Interfaces
{
    public interface IUsernameGenerator
    {
        string GenerateUsernameByEmail(string email);
    }
}
