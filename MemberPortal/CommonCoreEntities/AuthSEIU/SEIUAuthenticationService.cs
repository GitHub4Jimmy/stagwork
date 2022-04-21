using AspNetCore.LegacyAuthCookieCompat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonCoreEntities.FormsAuthentication;
using StagwellTech.SEIU.CommonCoreEntities.Services.UserSettingsAPI;
using StagwellTech.SEIU.CommonEntities.DNN;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public interface ICustomAuthenticationService
    {
        Task<DNNUser> AuthenticateUserAsync(string token);
    }

    public class SEIUAuthenticationService : ICustomAuthenticationService, ISEIUAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly SeiuContext context;
        private readonly SeiuDNNContext dnnContext;
        private readonly IRedisClient redis;

        public SEIUAuthenticationService(IConfiguration configuration, SeiuContext context, SeiuDNNContext dnnContext, IRedisClient redis)
        {
            this.configuration = configuration;
            this.context = context;
            this.dnnContext = dnnContext;
            this.redis = redis;
        }

        public string GetCurrentUserEmail(ClaimsPrincipal user)
        {
            var identityClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var email = identityClaim.Value;

            return email;
        }

        public string GetCurrentDocumentIdOrName(ClaimsPrincipal user)
        {
            var identityClaim = user.Claims.FirstOrDefault(c => c.Type == Constants.DocumentIdOrFileName);
            var email = identityClaim.Value;

            return email;
        }
        public bool IsSignatureUsed(ClaimsPrincipal user)
        {
            var identityClaim = user.Claims.FirstOrDefault(c => c.Type == Constants.SignatureUsed);
            bool signatureUsed;             
            if(bool.TryParse(identityClaim.Value, out signatureUsed)) {
                return signatureUsed;
            }
            return false;
        }
        public async Task<UserSettings> GetCurrentUserSettings(ClaimsPrincipal user)
        {
            var identityClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var dnnUserIdStr = identityClaim.Value;

            long dnnUserId;

            if (long.TryParse(dnnUserIdStr, out dnnUserId))
            {
                UserSettings settings = await this.context.UserSettings.Where(s => s.DNNUserId == dnnUserId).FirstOrDefaultAsync();
                return settings;
            }

            return null;
        }

        public async Task<MPPerson> GetCurrentPerson(ClaimsPrincipal user)
        {
            var identityClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var dnnUserIdStr = identityClaim.Value;

            long dnnUserId;

            if (long.TryParse(dnnUserIdStr, out dnnUserId))
            {
                var settings = await GetCurrentUserSettings(user);
                var person = await GetMPPersonFromCache(settings.PersonId);
                return person;
            }

            return null;
        }
        public async Task<MPPerson> GetMPPersonFromCache(string PersonId)
        {
            var key = "MPPerson_BY_PersonId-" + PersonId;
            MPPerson person;
            if (redis.TryGetValue(key, out person))
            {
                return person;
            }

            person = await this.context.MPPersons.FindAsync(long.Parse(PersonId));
            redis.Set(key, person);
            return person;
        }

        public async Task<DNNUser> AuthenticateUserAsync(string token)
        {
            var key = "DNNUser_BY_TOKEN-" + token;
            DNNUser user;
            if (redis.TryGetValue(key, out user))
            {
                return user;
            }

            user = await AuthenticateUserAsyncFromDB(token);
            redis.Set(key, user);
            return user;
        }

        public async Task<DNNUser> AuthenticateUserAsyncFromDB(string token)
        {
            // TODO: validate token
            try
            {
                var authCookie = await dnnContext.DNNAuthCookies
                    .Where(ac => token.StartsWith(ac.CookieValue))
                    .Include(ac => ac.User)
                    .FirstOrDefaultAsync();

                if(authCookie == null)
                {
                    return null;
                }

                var dnnUser = authCookie.User;

                if (dnnUser == null)
                {
                    return null;
                }

                string validationKey = "32B34178452B43ADC6D30047F5B92ED0FB89AA7D";
                string decryptionKey = "8832904761CD3C0B2BCAC983C94412837147CA24357A29A6";

                byte[] decryptionKeyBytes = HexUtils.HexToBinary(decryptionKey);
                byte[] validationKeyBytes = HexUtils.HexToBinary(validationKey);
                var legacyFormsAuthenticationTicketEncryptor = new LegacyFormsAuthenticationTicketEncryptorTripleDes(decryptionKeyBytes, validationKeyBytes, ShaVersion.Sha1, CompatibilityMode.Framework45);

                var decrypted = legacyFormsAuthenticationTicketEncryptor.DecryptCookie(token);

                if (decrypted == null)
                {
                    return null;
                }

                if (decrypted.Expired)
                {
                    return null;
                }

                if (dnnUser.Email != decrypted.Name)
                {
                    return null;
                }

                return dnnUser;

            }
            catch(Exception e)
            {
                Debug.WriteLine("Token decryption failed: " + e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            return null;
        }
    }
}
