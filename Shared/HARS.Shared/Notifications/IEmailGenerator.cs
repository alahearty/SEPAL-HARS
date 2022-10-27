using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Notifications
{
    public interface IEmailGenerator<TPayload>
    {
        Task<ActionResult> SendAsync(TPayload payload);
    }
}
