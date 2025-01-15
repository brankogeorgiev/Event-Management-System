using EMS.Service.Interface;
using MimeKit;
using EMS.Domain.Email;
using System.Net.Mail;
using EMS.Domain.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;

namespace EMS.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailMessage allMails)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress("Event Management System Application", "eventmanagementsystem2025@outlook.com"),
                Subject = allMails.Subject
            };

            emailMessage.From.Add(new MailboxAddress("Event Management System Application", "eventmanagementsystem2025@outlook.com"));

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = allMails.Content };

            emailMessage.To.Add(new MailboxAddress(allMails.MailTo, allMails.MailTo));

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = SecureSocketOptions.StartTls;

                    await smtp.ConnectAsync(_mailSettings.SmtpServer, 587, socketOptions);

                    if (!string.IsNullOrEmpty(_mailSettings.SmtpUserName))
                    {
                        await smtp.AuthenticateAsync(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                    }
                    await smtp.SendAsync(emailMessage);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"SMTP command failed: {ex.Message}");
                Console.WriteLine($"SMTP response: {ex.HResult}");
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"SMTP protocol error: {ex.Message}");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
