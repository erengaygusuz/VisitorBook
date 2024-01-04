namespace VisitorBook.Core.Abstract
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string toEmail, string subject, string bodyHtml);
    }
}
