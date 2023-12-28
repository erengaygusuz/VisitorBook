using System.Net.Mail;
using System.Net;
using VisitorBook.Backend.Core.Abstract;
using Microsoft.Extensions.Configuration;

namespace VisitorBook.Backend.BL.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = _configuration["EmailSettings:Host"];
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            smtpClient.Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_configuration["EmailSettings:Email"]);
            mailMessage.To.Add(toEmail);

            mailMessage.Subject = "Localhost | Şifre Sıfırlama Linki";

            mailMessage.Body = @$"<h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>
                                <p><a href='{resetPasswordEmailLink}'>Şifre yenileme linki</a></p>";

            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
