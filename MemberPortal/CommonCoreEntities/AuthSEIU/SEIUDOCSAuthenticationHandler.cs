using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using StagwellTech.SEIU.CommonEntities.Utils.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUDOCSAuthenticationHandler : AuthenticationHandler<SEIUDOCSAuthenticationOptions>
    {
        private readonly IServiceProvider serviceProvider;

        public SEIUDOCSAuthenticationHandler(
            IOptionsMonitor<SEIUDOCSAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock)
        {
            this.serviceProvider = serviceProvider;
        }

        protected new SEIUDOCSAuthenticationEvents Events
        {
            get { return (SEIUDOCSAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new SEIUDOCSAuthenticationEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if(string.IsNullOrWhiteSpace(authorizationHeader) && Request.Query.ContainsKey("authorization"))
            {
                authorizationHeader = Request.Query["authorization"];
            }

            TokenEncryptionObject decruptedSignature = null;

            if (string.IsNullOrWhiteSpace(authorizationHeader) && Request.Query.ContainsKey("signature") && Request.Query.ContainsKey("nonce") && Request.Query.ContainsKey("token"))
            {
                var eHelper = new EncryptionHelper();
                string _signature = Request.Query["signature"];
                string nonce = Request.Query["nonce"];
                string _token = Request.Query["token"];

                _signature = _signature.Replace(" ", "+");
                _token = _token.Replace(" ", "+");

                decruptedSignature = eHelper.DecryptToObject<TokenEncryptionObject>(_token, _signature);

                if (nonce.Equals(decruptedSignature.Nonce))
                {
                    authorizationHeader = decruptedSignature.Auth;
                }
            }

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            string token;

            if (authorizationHeader.StartsWith(Constants.SEIUDocumentsScheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                token = authorizationHeader.Substring(Constants.SEIUDocumentsScheme.Length).Trim();
            } else if(authorizationHeader.StartsWith(Constants.SEIUScheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                token = authorizationHeader.Substring(Constants.SEIUScheme.Length).Trim();
            } else
            {
                return AuthenticateResult.NoResult();
            }

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Credentials not provided");
            }

            try
            {
                var validateCredentialsContext = new ValidateDOCSTokenContext(Context, Scheme, Options)
                {
                    Token = token,
                    Signature = decruptedSignature
                };

                await Events.ValidateToken(validateCredentialsContext, serviceProvider);

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Succeeded)
                {
                    var ticket = new AuthenticationTicket(validateCredentialsContext.Principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Failure != null)
                {
                    return AuthenticateResult.Fail(validateCredentialsContext.Result.Failure);
                }

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                var authenticationFailedContext = new SEIUDOCSAuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Request.IsHttps)
            {
                const string insecureProtocolMessage = "Request is HTTP, SEIU Authentication will not respond.";
                Logger.LogInformation(insecureProtocolMessage);
                Response.StatusCode = 500;
                var encodedResponseText = Encoding.UTF8.GetBytes(insecureProtocolMessage);
                Response.Body.Write(encodedResponseText, 0, encodedResponseText.Length);
            }
            else
            {
                Response.StatusCode = 401;
                
                var headerValue = Constants.SEIUDocumentsScheme + $" USERNAME;PASSWORD";
                Response.Headers.Add(HeaderNames.WWWAuthenticate, headerValue);
            }

            return Task.CompletedTask;
        }
    }
}
