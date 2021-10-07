using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ShiftPlanning.Shifty.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(HttpClient httpClient, ILocalStorageService storageService)
        {
            _httpClient = httpClient;
            _localStorage = storageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtString = await _localStorage.GetItemAsync<string>("token");
            var user = ParseJwtString(jwtString);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtString);
            return new AuthenticationState(user);
        }

        public bool UpdateAuthState(string jwtString)
        {
            var user = ParseJwtString(jwtString);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            return true;
        }

        private static ClaimsPrincipal ParseJwtString (string jwtString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(jwtString))
                return new ClaimsPrincipal();

            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtString);
            return (DateTime.UtcNow < token.ValidTo) switch
            {
                true => new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "bearerToken")), //Needs the string passed as well, otherwise the user is not set to authenticated
                false => new ClaimsPrincipal()
            };
        }
    }
}