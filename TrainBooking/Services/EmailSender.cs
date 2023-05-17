using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using System.Net.Http;
using System.Net.Mail;
using Attachment = SendGrid.Helpers.Mail.Attachment;
using Syncfusion.Drawing;

namespace TrainBooking.Util.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            // GENERATE PDF
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics g = page.Graphics;
            PdfBrush brush = new PdfSolidBrush(Color.Black);
            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16f);
            PdfFont standardFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12f);
            g.DrawString("Thank you for your purchase!", titleFont, brush, new PointF(20, 20));
            g.DrawString(message, standardFont, brush, new PointF(20, 50));
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            stream.Position = 0;

            // SEND MAIL
            string content = "Thank you for choosing to travel with us. This is a confirmation of the booking you have made with us and confirms the travel details of your transport contract. Please take a moment to check that the details are correct.";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("trainbookingnoreply@gmail.com", "trainbooking-noreply"),
                Subject = subject,
                PlainTextContent = content,
                HtmlContent = content
            };
            msg.AddAttachment("PdfAttachment.pdf", Convert.ToBase64String(stream.ToArray()));
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}