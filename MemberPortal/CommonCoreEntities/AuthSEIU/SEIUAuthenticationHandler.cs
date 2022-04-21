using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUAuthenticationHandler : AuthenticationHandler<SEIUAuthenticationOptions>
    {
        private readonly IServiceProvider serviceProvider;

        public SEIUAuthenticationHandler(
            IOptionsMonitor<SEIUAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock)
        {
            this.serviceProvider = serviceProvider;
        }

        protected new SEIUAuthenticationEvents Events
        {
            get { return (SEIUAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new SEIUAuthenticationEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if(string.IsNullOrWhiteSpace(authorizationHeader) && Request.Query.ContainsKey("authorization"))
            {
                authorizationHeader = Request.Query["authorization"];
            }

            //if (string.IsNullOrWhiteSpace(authorizationHeader) && Request.Query.ContainsKey("signature") && Request.Query.ContainsKey("nonce"))
            //{
            //    var eHelper = new EncryptionHelper();
            //    string signature = Request.Query["signature"];
            //    string nonce = Request.Query["nonce"];
            //    EncryptionHelper.TokenEncryptionObject decruptedSignature = eHelper.DecryptToObject<EncryptionHelper.TokenEncryptionObject>(signature);

            //    if(nonce.Equals(decruptedSignature.Nonce))
            //    {
            //        authorizationHeader = decruptedSignature.Auth;
            //    }
            //}

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(Constants.SEIUScheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            string token = authorizationHeader.Substring(Constants.SEIUScheme.Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Credentials not provided");
            }

            try
            {
                var validateCredentialsContext = new ValidateTokenContext(Context, Scheme, Options)
                {
                    Token = token
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
                var authenticationFailedContext = new SEIUAuthenticationFailedContext(Context, Scheme, Options)
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

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Request.IsHttps)
            {
                const string insecureProtocolMessage = "Request is HTTP, SEIU Authentication will not respond.";
                Logger.LogInformation(insecureProtocolMessage);
                Response.StatusCode = 500;
                var encodedResponseText = Encoding.UTF8.GetBytes(insecureProtocolMessage);
                await Response.Body.WriteAsync(encodedResponseText, 0, encodedResponseText.Length);
            }
            else
            {
                Response.StatusCode = 401;
                
                var headerValue = Constants.SEIUScheme + $" USERNAME;PASSWORD";
                Response.Headers.Add(HeaderNames.WWWAuthenticate, headerValue);
            }

        }
    }
}
