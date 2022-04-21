using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using DotNetNuke.Data;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Services.Authentication;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Member;
using StagwellTech.SEIU.CommonEntities.User;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Auth0UserInfo = Auth0.AuthenticationApi.Models.UserInfo;
using DNNUserInfo = DotNetNuke.Entities.Users.UserInfo;
using StagwellTech.SEIU.CommonEntities.Auth0;
using StagwellTech.SEIU.CommonEntities.Auth0.Entities;
using StagwellTech.SEIU.CommonEntities.Portal.Registration;
using System.Diagnostics;
using DotNetNuke.Security.Roles;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using StagwellTech.ServiceBusRPC;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using StagwellTech.SEIU.CommonDNNEntities.DataProviders;
using System.Web.SessionState;
using StagwellTech.SEIU.CommonDNNEntities.Auth0;

namespace Dnn.Authentication.Auth0.Components
{

    public class Auth0Client : Auth0ClientBase
    {
        private static readonly DataProvider Data = DataProvider.Instance();

        public string AuthToken { get; private set; }

        public string VerificationCode => HttpContext.Current.Request.Params[_OAuthCodeKey];
        public string RedirectUrl => HttpContext.Current.Request.Params[_OAuthRedirectKey];

        private static EntitlementsClient _entitlementClient = null;

        public Auth0Client(int portalId) : base(Auth0ConfigBase.GetConfig(portalId))
        {
            GetEntitlementsClient();
        }

        private static EntitlementsClient GetEntitlementsClient()
        {
            if(_entitlementClient != null)
            {
                return _entitlementClient;
            }

            _entitlementClient = EntitlementsClient.Instance;

            return _entitlementClient;

        }

        public bool HaveVerificationCodeAndRedirectUrl()
        {
            return !string.IsNullOrEmpty(VerificationCode) && !string.IsNullOrEmpty(RedirectUrl);
        }

        public async Task<AuthorisationResult> ExchangeCodeForTokenAsync()
        {
            return await ExchangeCodeForTokenAsync(VerificationCode, RedirectUrl);
        }

        public async Task<RegistrationForm> Signup(RegistrationFormClient registrationFormClient, DNNUserInfo objUser, RegistrationForm form)
        {
            if (form.PersonNotFound || form.PersonId == null)
            {
                throw new Exception("Missing information");
            }

            UserCreateStatus result = UserController.CreateUser(ref objUser);
            Console.WriteLine("result => " + result.ToString());
            Debug.WriteLine("result => " + result.ToString());


            // Need to check status here, just not sure which should be. probably 13 - Success but there are others which makes sence as well...

            var apiClient = new AuthenticationApiClient(Config.TenantDomain);
            var signupUserRequest = new SignupUserRequestWithName
            {
                ClientId = Config.ClientId,
                Email = form.Email,
                Password = form.Password,
                Connection = Config.ConnectionName,
                FirstName = objUser.FirstName,
                LastName = objUser.LastName,
                FullName = objUser.DisplayName,
                UserMetadata = new
                {
                    PersonId = form.PersonId.ToString()
                }
            };
            try
            {
                var auth0Response = await apiClient.SignupUserAsync(signupUserRequest);
                form.Auth0Id = _Service.ToLower() + "|" + auth0Response.Id;

                await UpdatePersonalInfo(form.Auth0Id, objUser.FirstName, objUser.LastName, objUser.DisplayName, form.PersonId);

            } catch(Exception e)
            {
                UserController.DeleteUser(ref objUser, false, true);
                UserController.RemoveDeletedUsers(objUser.PortalID);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            // DNN itself doesn't check the result. I guess it should be 0 or 1 for success.
            var authAdded = Data.AddUserAuthentication(objUser.UserID, _Service, form.Auth0Id, -1);

            form = await registrationFormClient.Complete(form, objUser.UserID);

            return form;

        }

        public virtual bool AuthenticateDnnUser(HttpRequest Request, HttpSessionState Session, Auth0UserInfo user, PortalSettings settings, string IPAddress, Action<UserAuthenticatedEventArgs> onAuthenticated)
        {
            var loginStatus = UserLoginStatus.LOGIN_FAILURE;
            
            var objUserInfo = UserController.ValidateUser(
                settings.PortalId,
                user.UserId,
                "",
                _Service,
                user.UserId,
                settings.PortalName,
                IPAddress,
                ref loginStatus);

            var portalSettings = PortalController.Instance.GetCurrentPortalSettings();

            var context = new SEIUDNNContext(Request, Session, objUserInfo.UserID);

            var task = Task.Run(async () => {
                try
                {
                    await EntitlementHandler.Instance.HandleEntitlements(objUserInfo, portalSettings, context);
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.InnerException);
                    loginStatus = UserLoginStatus.LOGIN_FAILURE;
                }
            });
            task.Wait();

            if(loginStatus == UserLoginStatus.LOGIN_FAILURE)
            {
                return false;
            }

            //Raise UserAuthenticated Event
            var eventArgs = new UserAuthenticatedEventArgs(objUserInfo, user.UserId, loginStatus, _Service)
            {
                AutoRegister = false
            };

            var profileProperties = new NameValueCollection();

            if (objUserInfo == null || (string.IsNullOrEmpty(objUserInfo.Email) && !string.IsNullOrEmpty(user.Email)))
            {
                profileProperties.Add("Email", user.Email);
            }

            eventArgs.Profile = profileProperties;

            onAuthenticated(eventArgs);
            return true;
        }

        private bool IsValidCultureName(string name)
        {
            return
                CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Any(c => c.Name == name);
        }

    }
}