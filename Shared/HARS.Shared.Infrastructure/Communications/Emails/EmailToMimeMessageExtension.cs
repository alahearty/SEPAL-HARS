using System.Net.Mail;

namespace HARS.Shared.Infrastructure.Communications.Emails
{
    public static class EmailToMimeMessageExtension
    {
        public static MailMessage AsMimeMessage(this Email email)
        {
            MailMessage message = new MailMessage();
            email.Reciepients.ForEach(reciepient => message.To.Add(reciepient));

            return message;
        }
    }
}