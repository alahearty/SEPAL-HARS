using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shared = HARS.UserMangement.AccessManager;

namespace Admin.Domain.UsersManagement
{
    public interface IAccessManager : shared.IAccessManager<HARSUser>//, HARSUserRole>
    {
        string HashPassword(string plainPassword);
        Task<bool> AdminAccountSeeded();
        Task<bool> DoesUserExistAsync(string email);
    }
}
