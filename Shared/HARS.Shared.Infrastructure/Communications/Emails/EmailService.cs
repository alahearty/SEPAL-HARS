using HARS.Shared.Contracts;
using HARS.Shared.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Communications.Emails
{
    public class EmailService : IEmailServer
    {
        private MimeMessage CreateMimeMessageFromEmailMessage(Email message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("SEPAL HARS", message.From));
            mimeMessage.To.AddRange(message.Reciepients.Select(reciever => new MailboxAddress(reciever, reciever)));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };
            return mimeMessage;
        }

        public EmailService(IEmailServiceConfiguration emailConfiguration)
        {
            EmailConfiguration = emailConfiguration;
        }

        public async Task<ActionResult> Send(Email mail)
        {
            new Thread(async () =>
            {
                var mimeMessage = CreateMimeMessageFromEmailMessage(mail);
                try
                {
                    using var smtpClient = new SmtpClient();
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await smtpClient.ConnectAsync(EmailConfiguration.Host, EmailConfiguration.Port, SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(EmailConfiguration.UserName, EmailConfiguration.Password);
                    await smtpClient.SendAsync(mimeMessage);
                    await smtpClient.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email", ex);
                    //return ActionResult.Failed().AddError(ex.Message);
                }
            }).Start();

            return await Task.FromResult(ActionResult.Success());
        }


        public string Sender => EmailConfiguration.From;

        public IEmailServiceConfiguration EmailConfiguration { get; }
    }
}
