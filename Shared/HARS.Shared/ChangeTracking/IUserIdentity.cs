using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.ChangeTracking
{
    public interface IUserIdentity
    {
        string Email { get; set; }

        Guid UserId { get; set; }
    }
}
