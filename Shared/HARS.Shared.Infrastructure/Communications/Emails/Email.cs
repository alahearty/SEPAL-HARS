using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Communications.Emails
{
    public class Email
    {
        public Email(string from, string body, string subject)
        {
            From = from;
            Body = body;
            Subject = subject;
        }
        public string From { get; set; }
        public List<string> Reciepients { get; set; } = new List<string>();
        public string Body { get; set; }
        public string Subject { get; set; }
    }

    public static class EmailExtensions
    {
        public static Email AddReciepient(this Email email, string recipient)
        {
            email.Reciepients.Add(recipient);
            return email;
        }
    }
}
