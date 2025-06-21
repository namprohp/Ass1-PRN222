using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("FUNews Management", _configuration["EmailSettings:SenderEmail"]));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = message };
        email.Body = bodyBuilder.ToMessageBody();

        // ✅ Đảm bảo sử dụng đúng SmtpClient của MailKit
        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
