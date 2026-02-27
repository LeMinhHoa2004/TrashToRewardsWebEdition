using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace TrashToRewardsV2.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachmentBytes, string fileName);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:Username"], "Hệ Thống Trash To Rewards"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc bỏ qua
                Console.WriteLine($"[Email Error] {ex.Message}");
                // KHÔNG throw để không ảnh hưởng hệ thống
            }
        }
        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachmentBytes, string fileName)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:Username"], "Hệ Thống Trash To Rewards"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);
                mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), fileName));

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Email Error] {ex.Message}");
                // KHÔNG throw ra ngoài
            }
        }
    }
}
