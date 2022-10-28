using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Authentication.JWTAuthentication
{
    public interface IJWTTokenProvider
    {
        string MakeToken(string email, DateTime? tokenExpiry = null);
    }
}
