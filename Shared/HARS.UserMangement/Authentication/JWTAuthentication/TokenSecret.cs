using HARS.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Authentication.JWTAuthentication
{
    public class TokenSecret : ITokenSecret
    {
        public string CypherCrescentSecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
