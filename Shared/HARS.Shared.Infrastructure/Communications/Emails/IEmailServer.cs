using HARS.Shared.Utilities;

namespace HARS.Shared.Infrastructure.Communications.Emails
{
    public interface IEmailServer
    {
        string Sender { get; }
        Task<ActionResult> Send(Email mail);
    }
}
