using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Core
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }

    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string name) : base(name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return NormalizedName;
        }
    }

    public enum PermissionAccess
    {
        None,
        Allow,
        Deny
    }

    public interface IUser
    {
        public string FirstName { get; }
        public string LastName { get; }
        public bool IsLockedOut { get; }
    }
}
