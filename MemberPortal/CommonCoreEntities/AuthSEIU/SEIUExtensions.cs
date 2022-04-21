using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public static class SEIUExtensions
    {
        public static IServiceCollection AddSEIUAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection
            .AddAuthorization(options =>
            {
                options.AddSEIUPolicy();
            })
            .AddAuthentication(Constants.SEIUScheme)
            .AddScheme<SEIUAuthenticationOptions, SEIUAuthenticationHandler>(Constants.SEIUScheme, options =>
            {
                options.Events = new SEIUAuthenticationEvents
                {
                    OnValidateToken = async (context, serviceProvider) =>
                    {
                        var authenticationService = serviceProvider.GetService<ICustomAuthenticationService>();

                        var authenticatedUser = await authenticationService.AuthenticateUserAsync(context.Token);

                        if (authenticatedUser != null)
                        {
                            var claims = new[]
                            {
                                    new Claim(
                                        ClaimTypes.NameIdentifier,
                                        authenticatedUser.UserID.ToString(),
                                        ClaimValueTypes.Integer64,
                                        context.Options.ClaimsIssuer),

                                    new Claim(
                                        ClaimTypes.Name,
                                        authenticatedUser.DisplayName,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer),

                                    new Claim(
                                        ClaimTypes.Email,
                                        authenticatedUser.Email,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer),
                                    
                                };

                            context.Principal = new ClaimsPrincipal(
                                new ClaimsIdentity(claims, context.Scheme.Name));

                            context.Success();

                        }

                    }
                };
            });

            return serviceCollection;
        }

        public static AuthorizationOptions AddSEIUPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Constants.SEIUScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.SEIUPolicy, policy);
            return options;
        }

        public static IServiceCollection AddSEIUDOCSAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection
            .AddAuthorization(options =>
            {
                options.AddSEIUDOCSPolicy();
            })
            .AddAuthentication(Constants.SEIUDocumentsScheme)
            .AddScheme<SEIUDOCSAuthenticationOptions, SEIUDOCSAuthenticationHandler>(Constants.SEIUDocumentsScheme, options =>
            {
                options.Events = new SEIUDOCSAuthenticationEvents
                {
                    OnValidateToken = async (context, serviceProvider) =>
                    {
                        var authenticationService = serviceProvider.GetService<ICustomAuthenticationService>();

                        var authenticatedUser = await authenticationService.AuthenticateUserAsync(context.Token);
                        
                        if (authenticatedUser != null)
                        {
                            var claims = new List<Claim>();
                            //var claims = new[]
                            //{
                            claims.Add(new Claim(
                                        ClaimTypes.NameIdentifier,
                                        authenticatedUser.UserID.ToString(),
                                        ClaimValueTypes.Integer64,
                                        context.Options.ClaimsIssuer));

                            claims.Add(new Claim(
                                        ClaimTypes.Name,
                                        authenticatedUser.DisplayName,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer));

                            claims.Add(new Claim(
                                        ClaimTypes.Email,
                                        authenticatedUser.Email,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer));

                                //};

                            if(context.Signature != null)
                            {
                                
                                claims.Add(new Claim(
                                            Constants.DocumentIdOrFileName,
                                            context.Signature.DocumentIdOrFileName,
                                            ClaimValueTypes.String,
                                            context.Options.ClaimsIssuer));

                                claims.Add(new Claim(
                                            Constants.SignatureUsed,
                                            true.ToString(),
                                            ClaimValueTypes.Boolean,
                                            context.Options.ClaimsIssuer));

                            } else
                            {

                                claims.Add(new Claim(
                                            Constants.SignatureUsed,
                                            false.ToString(),
                                            ClaimValueTypes.Boolean,
                                            context.Options.ClaimsIssuer));

                            }

                            context.Principal = new ClaimsPrincipal(
                                new ClaimsIdentity(claims, context.Scheme.Name));

                            context.Success();

                        }

                    }
                };
            });

            return serviceCollection;
        }

        public static AuthorizationOptions AddSEIUDOCSPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Constants.SEIUDocumentsScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.SEIUDocumentsPolicy, policy);
            return options;
        }
    }
}
