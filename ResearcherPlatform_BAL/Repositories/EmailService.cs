using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Models;

public class EmailService : IEmailService
{
    private readonly System.Net.Mail.SmtpClient _smtpClient;
    private readonly string _senderEmail;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        var settings = emailSettings.Value;

        _smtpClient = new SmtpClient(settings.SmtpServer, settings.SmtpPort ?? 0)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
            EnableSsl = settings.EnableSsl
        };

        _senderEmail = settings.SenderEmail;
    }

    public async Task SendPasswordResetEmailAsync(string email, string callbackUrl)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_senderEmail),
            Subject = "Password Reset",
            Body = $"Click the following link to reset your password: {callbackUrl}",
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        await _smtpClient.SendMailAsync(mailMessage);
    }
}