using HARS.Shared.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Authentication.JWTAuthentication
{
    public class JWTTokenProvider : IJWTTokenProvider
    {
        public JWTTokenProvider(ITokenSecret tokenSecret)
        {
            SecretAPIKeys = tokenSecret;
        }

        public string MakeToken(string email, DateTime? tokenExpiry = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Aud, SecretAPIKeys.Audience),
                new Claim(JwtRegisteredClaimNames.Iss, SecretAPIKeys.Issuer),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretAPIKeys.CypherCrescentSecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claims,
                                             signingCredentials: signingCredentials,
                                             expires: tokenExpiry);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public ITokenSecret SecretAPIKeys { get; }

    }
}
