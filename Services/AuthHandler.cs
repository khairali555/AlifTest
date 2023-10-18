using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AlifTestTask.Services
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly AuthService _authService;

        public AuthHandler(
            AuthService authService, 
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            UrlEncoder encoder, 
            ILoggerFactory logger,
            ISystemClock clock) : base(options,logger,encoder,clock)
        {
            _authService = authService;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Autheticate"] = "Basic";
            return base.HandleChallengeAsync(properties);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var hash = credentials.LastOrDefault();

                if (!_authService.CheckUser(username, hash))
                {
                    throw new ArgumentException($"Invalid username or passrword {hash}");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,username)
            };

            var isentity = new ClaimsIdentity(claims,Scheme.Name);
            var principal = new ClaimsPrincipal(isentity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
