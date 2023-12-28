namespace VisitorBook.Core.Abstract
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
    }
}
