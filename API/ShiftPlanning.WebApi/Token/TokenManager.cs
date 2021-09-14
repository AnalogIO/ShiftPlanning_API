using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShiftPlanning.Common.Configuration;

namespace ShiftPlanning.WebApi.Token
{
    public class TokenManager : ITokenManager
    {
        private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
        private const string AppliesToAddress = "https://www.analogio.dk/";
        private readonly IdentitySettings _identitySettings;

        public TokenManager(IdentitySettings identitySettings)
        {
            _identitySettings = identitySettings;
        }

        /// <summary>
        /// Generates and return a JWT token.
        /// </summary>
        /// <returns></returns>
        public string GenerateToken()
        {
            var securityToken = TokenHandler.CreateToken(new SecurityTokenDescriptor { IssuedAt = DateTime.Now, Expires = DateTime.Now.AddHours(24) });
            return TokenHandler.WriteToken(securityToken);
        }

        public string GenerateLoginToken()
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identitySettings.TokenKey));
            var securityToken = TokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(_identitySettings.TokenAgeHour),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, "Login"),
                }),
                Issuer = "AnalogIO",
                Audience = AppliesToAddress,
                SigningCredentials = new SigningCredentials(
                    symmetricKey, 
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256")
            });
            return TokenHandler.WriteToken(securityToken);
        }

        public bool ValidateLoginToken(string token)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identitySettings.TokenKey));
            try
            {
                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = AppliesToAddress,
                    ValidIssuer = "self",
                    IssuerSigningKey = symmetricKey
                };

                SecurityToken securityToken;
                var principal = TokenHandler.ValidateToken(token, validationParameters, out securityToken);
                var valid = principal.Identities.First().Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Login");
                if (valid && securityToken.ValidTo > DateTime.UtcNow)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}