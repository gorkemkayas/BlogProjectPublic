namespace BlogProject.Services.Abstract
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string resetEmailLink, string receiverEmail);
    }
}
