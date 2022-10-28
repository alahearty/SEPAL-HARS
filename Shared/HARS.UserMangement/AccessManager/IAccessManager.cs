using HARS.UserMangement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.AccessManager
{
    public interface IAccessManager<TUser> where TUser : ApplicationUser//, TRole> where TUser : ApplicationUser where TRole : ApplicationRole
    {
        IUserRepository<TUser> UserRepository { get; }
        //IRoleRepository<TRole> RoleRepository { get; }
    }

    public interface IAppLoginToken
    {
        string TokenString { get; }
        string Email { get; }
    }
}
