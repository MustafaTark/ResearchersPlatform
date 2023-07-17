using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ResearchersPlatform_BAL.Repositories
{
    //public class EmailService : IEmailService
    //{
    //    private readonly EmailSettings _emailSettings;

    //    public EmailService(IOptions<EmailSettings> emailSettings)
    //    {
    //        _emailSettings = emailSettings.Value;
    //    }

    //    public async Task SendEmailAsync(string email, string subject, string body)
    //    {
    //        var message = new MimeMessage();
    //        message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
    //        message.To.Add(new MailboxAddress("", email));
    //        message.Subject = subject;

    //        var bodyBuilder = new BodyBuilder();
    //        bodyBuilder.HtmlBody = body;
    //        message.Body = bodyBuilder.ToMessageBody();

    //        using (var client = new SmtpClient())
    //        {
    //            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
    //            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
    //            await client.SendAsync(message);
    //            await client.DisconnectAsync(true);
    //        }
    //    }
    //}
}
