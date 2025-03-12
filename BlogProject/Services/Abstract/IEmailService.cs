namespace BlogProject.Services.Abstract
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string resetEmailLink, string receiverEmail);
        Task SendPasswordChangedNotificationAsync(string subject, string receiverEmail);
    }
}
