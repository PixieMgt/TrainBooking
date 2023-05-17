using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace TrainBooking.Services
{
    public interface IEmailSender2
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSender2 : IEmailSender2
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender2(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(
            string email, string subject, string message)
        {
            var mail = new MailMessage();  // aanmaken van een mail‐object
            mail.To.Add(new MailAddress(email));
            mail.From = new
                    MailAddress("van.haverbeke.bram@gmail.com");  // hier komt jullie Gmail‐adres
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials =
                        new NetworkCredential(_emailSettings.Sender,
                                                _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
