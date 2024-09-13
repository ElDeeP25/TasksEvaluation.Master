using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using TasksEvaluation.Infrastructure.Helpers;

public class EmailSender : IEmailSender
{
    private readonly MailSettings _mailSettings;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmailSender(IOptions<MailSettings> mailSettings, IWebHostEnvironment webHostEnvironment)
    {
        _mailSettings = mailSettings?.Value ?? throw new ArgumentNullException(nameof(mailSettings));
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrEmpty(email))
            return;

        var message = new MailMessage
        {
            From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
            Subject = subject,
            Body = $"<html><body>{htmlMessage}</body></html>",
            IsBodyHtml = true
        };
        message.To.Add(email);

        using var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
        {
            Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
            EnableSsl = true // تأكد من تمكين SSL
        };

        await smtpClient.SendMailAsync(message);
    }
}
