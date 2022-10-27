using HARS.Shared.Contracts;

namespace HARS.Shared.Infrastructure.Communications.Emails
{
    public sealed class EmailServiceConfiguration : IEmailServiceConfiguration
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
    }
}