using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public interface ISEIUAuthenticationService
    {
        Task<MPPerson> GetCurrentPerson(ClaimsPrincipal user);
        Task<UserSettings> GetCurrentUserSettings(ClaimsPrincipal user);
        bool IsSignatureUsed(ClaimsPrincipal user);
        string GetCurrentDocumentIdOrName(ClaimsPrincipal user);
        string GetCurrentUserEmail(ClaimsPrincipal user);
    }
}