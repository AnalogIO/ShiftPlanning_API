using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ShiftPlanning.Shifty.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService storageService)
        {
            _localStorage = storageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenString = await _localStorage.GetItemAsync<string>("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            
            if (tokenHandler.CanReadToken(tokenString))
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
                var identity = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
            }

            throw new System.NotImplementedException();
        }
    }
}