using System;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;

namespace Data.Token
{
    public static class TokenManager
    {
        private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
        private const string AppliesToAddress = "https://www.analogio.dk/";

        /// <summary>
        /// Generates and return a JWT token.
        /// </summary>
        /// <returns></returns>
        public static string GenerateToken()
        {
            var securityToken = TokenHandler.CreateToken(new SecurityTokenDescriptor { Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(24)) });
            return TokenHandler.WriteToken(securityToken);
        }

        public static string GenerateLoginToken()
        {
            var symmetricKey = GetBytes(ConfigurationManager.AppSettings["TokenKey"]);
            var securityToken = TokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(24)),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, "Login"),
                }),
                TokenIssuerName = "self",
                AppliesToAddress = AppliesToAddress,
                SigningCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(symmetricKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256")
            });
            return TokenHandler.WriteToken(securityToken);
        }

        public static bool ValidateLoginToken(string token)
        {
            var symmetricKey = GetBytes(ConfigurationManager.AppSettings["TokenKey"]);
            try
            {
                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = AppliesToAddress,
                    ValidIssuer = "self",
                    IssuerSigningToken = new BinarySecretSecurityToken(symmetricKey)
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

        private static byte[] GetBytes(string input)
        {
            var bytes = new byte[input.Length * sizeof(char)];
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;

        }
    }
}