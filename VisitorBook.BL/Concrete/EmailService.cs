using System.Net.Mail;
using System.Net;
using VisitorBook.Core.Abstract;
using Microsoft.Extensions.Configuration;

namespace VisitorBook.BL.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(string toEmail, string subject, string bodyHtml)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = _configuration["EmailSettings:Host"];
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            smtpClient.Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]);
            smtpClient.EnableSsl = Convert.ToBoolean(_configuration["EmailSettings:SSLCertificate"]);

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_configuration["EmailSettings:Email"]);
            mailMessage.To.Add(toEmail);

            mailMessage.Subject = subject;

            mailMessage.Body = bodyHtml;

            mailMessage.IsBodyHtml = true;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
