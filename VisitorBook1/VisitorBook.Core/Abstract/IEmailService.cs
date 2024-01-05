namespace VisitorBook.Core.Abstract
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string bodyHtml);
    }
}
