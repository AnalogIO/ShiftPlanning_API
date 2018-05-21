using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using API.Authorization;
using Data.Token;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using System.Web;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "*"));

            config.Filters.Add(new ExceptionToHttpStatusFilterAttribute());
            config.Filters.Add(new AnalogAuthenticationFilter());

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }

    public class AnalogAuthenticationFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authManager = UnityConfig.GetConfiguredContainer().Resolve<IAuthManager>();
            if (!await AuthenticateRoles(context, authManager))
            {
                AuthenticateApp(context, authManager);
            }
        }

        private async Task<bool> AuthenticateRoles(HttpAuthenticationContext context, IAuthManager authManager)
        {
            var token = context.Request.Headers.Authorization?.ToString();

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var principal = await AuthenticateJwtToken(token, authManager);

                if (principal != null)
                {
                    context.Principal = principal;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private void AuthenticateApp(HttpAuthenticationContext context, IAuthManager authManager)
        {
            var token = context.Request.Headers.Authorization?.ToString();

            if (token != null)
            {
                if (authManager.ValidateOrganizationApiKey(token))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "Application")
                    };

                    var identity = new ClaimsIdentity(claims, "Jwt");
                    IPrincipal user = new ClaimsPrincipal(identity);

                    context.Principal = user;
                    HttpContext.Current.Items["Audit"] = "Application";
                }
            }
        }

        private Task<IPrincipal> AuthenticateJwtToken(string token, IAuthManager authManager)
        {
            if (TokenManager.ValidateLoginToken(token))
            {
                var roles = authManager.GetRoles(token);
                var claims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                HttpContext.Current.Items["Audit"] = "User";

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
