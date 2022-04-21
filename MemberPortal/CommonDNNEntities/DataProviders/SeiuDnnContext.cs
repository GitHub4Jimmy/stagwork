using Auth0.ManagementApi.Models;
using Newtonsoft.Json.Linq;
using StagwellTech.SEIU.CommonEntities;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Member;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.User;
using StagwellTech.ServiceBusRPC;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls.WebParts;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPFundJob;
using StagwellTech.SEIU.CommonDNNEntities.Pension;
using StagwellTech.SEIU.CommonEntities.DBO.Person.Pension;
using static StagwellTech.SEIU.CommonDNNEntities.Pension.Pension401KLoader;
using ReadOnlyEvent = StagwellTech.SEIU.CommonEntities.ReadOnly.Event.Event;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using Twilio.Rest.Preview.Sync.Service.SyncList;
using StagwellTech.SEIU.CommonEntities.DBO.Person;
using Microsoft.IdentityModel.Logging;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.Utils;
using TrainingFund.Shared.ViewModels;
using Twilio.TwiML.Voice;
using StagwellTech.SEIU.CommonEntities.Med;
using StagwellTech.SEIU.CommonEntities.RefLang;
using StagwellTech.SEIU.CommonEntities.DBO.MedProviders;
using StagwellTech.SEIU.CommonEntities.UnionContract;
using StagwellTech.SEIU.CommonEntities.Document;
using StagwellTech.SEIU.CommonEntities.DBO.MPHealthProgram;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Dependent;
using StagwellTech.SEIU.CommonEntities.JohnHancock;
using StagwellTech.SEIU.CommonEntities.DBO.KnowledgeArticles;
using Newtonsoft.Json;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPEligibility;
using StagwellTech.SEIU.CommonEntities.DTO.MPEligibility;
using StagwellTech.SEIU.CommonEntities.DBO.Alert;
using StagwellTech.SEIU.CommonEntities.DTO.Alert;
using DotNetNuke.Security;
using DotNetNuke.Services.Log.EventLog;
using static DotNetNuke.Services.Log.EventLog.EventLogController;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace StagwellTech.SEIU.CommonDNNEntities.DataProviders
{
    public class SEIUDNNContext : SessionCacheLayer
    {
        public SEIUDNNContext(HttpRequest Request, HttpSessionState Session, int DNNUserId) : base(Request, Session, DNNUserId) { }

        public async Task<int> GetPersonIdAsync()
        {
            var settings = await GetUserSettingsAsync();
            int personId;
            if (int.TryParse(settings.PersonId, out personId))
            {
                return personId;
            }
            throw new Exception("Unable to retrieve PersonId. Either couldn't get response from APIs or value returned was not an integer.");
        }

        public async Task<UserSettings> SetUserSettingsAsync(UserSettings value = null)
        {
            return await SetObjectAsync(Key.USER_SETTINGS, () => GetUserSettingsAsync(), value);
        }

        public async Task<UserSettings> GetUserSettingsAsync()
        {
            return await GetObjectAsyncSession(Key.USER_SETTINGS, () => UserSettingsClient.Instance.getByDNNUserId(DNNUserId));
        }

        public async Task<List<MPAlert>> GetPersonAlertsByRulesAsync(int personId, List<AlertRules> rules, int numberOfAlerts = 2)
        {
            string key = $"MPAlerts_{personId}_{JsonConvert.SerializeObject(rules)}_{numberOfAlerts}";
            return await GetVariableObjectAsyncSession(key, () => AlertClient.Instance.GetPersonAlertsByRules(personId, rules, numberOfAlerts));
        }

        public async Task<List<ReadOnlyEvent>> GetDefaultUpcomingEvents(string personId)
        {
            return await GetObjectAsyncSession(Key.GET_DEFAULT_UPCOMMING_EVENTS, () => EventClient.Instance.GetDefaultUpcoming(personId));
        }

        public async Task<MemberDashboardData> LoadMemberDashboard()
        {
            MemberDashboardData data = await GetObjectAsyncSession(Key.MEMBER_DASHBOARD_DATA, () => UserSettingsClient.Instance.LoadMemberDashboard(DNNUserId));

            if (data != null)
            {
                SetInSession(Key.USER_SETTINGS, data.settings);
                SetInSession(Key.ENTITLEMENTS, data.Entitlements);
                SetInSession(Key.PERSON_DISTRICT, data.MemberDistrict);
                SetInSession(Key.MPPERSON, data.MPPerson);
                SetInSession(Key.MPPERSONEXT, data.MPPersonExt);
                SetInSession(Key.ACTIVE_MP_FUND_JOBS, data.ActiveJobs);
                SetInSession(Key.MP_ELIGIBILITIES, data.MPEligibilities);
                SetInSession(Key.PERSON_PLAN_CODES, data.PlanCodes);
                SetInSession(Key.PERSON_PLAN_IDS, data.PlanIds);
                SetInSession(Key.DEPENDENTS, data.Dependents);

                SetPension401K(data.pensionSrsp);
                SetInSession(Key.PENSION, data.pensionPen);

                SetIfHasUnseen(new HasUnseenResponse() { HasUnseen = data.HasUnseenDocuments });
            }

            return data;
        }

        public async Task<bool> GetOmniChatAvailability(long personId)
        {
            try
            {
                var result = await GetObjectAsyncSession(Key.OMNI_CHAT_AVAILABILITY, () => ContactsClient.Instance.GetByPersonId(personId));
                return result != null && Guid.Empty != result.ContactId;
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<UserSettings> GetRefreshedUserSettingsAsync()
        {
            var key = Key.USER_SETTINGS;
            var value = await UserSettingsClient.Instance.getByDNNUserId(DNNUserId);
            SetInSession(key, value);
            return value;
        }

        public async Task<MPPerson> GetMPPersonAsync()
        {
            var personId = (await GetPersonIdAsync()).ToString();
            //var getter = ;

            return await GetObjectAsyncSession(Key.MPPERSON, () => MPPersonClient.Instance.Get(personId));
        }
        public async Task<MPPerson> GetRefreshedMPPersonAsync()
        {
            var key = Key.MPPERSON;
            var personId = (await GetPersonIdAsync()).ToString();
            var person = await MPPersonClient.Instance.Get(personId);
            SetInSession(key, person);
            return person;
        }
        public void SetIfHasUnseen(HasUnseenResponse data)
        {
            Debug.WriteLine("SetIfHasUnseen =================================");
            SetInSession(Key.CHECK_IF_HAS_UNSEEN, data);
        }

        public async Task<HasUnseenResponse> CheckIfHasUnseen()
        {
            Debug.WriteLine("CheckIfHasUnseen =================================");
            var personId = (await GetPersonIdAsync()).ToString();
            //var getter = ;

            return await GetObjectAsyncSession(Key.CHECK_IF_HAS_UNSEEN, () => DocumentClient.Instance.CheckIfHasUnseenResponse(personId));
        }

        public async Task<MPPersonExt> GetMPPersonExtAsync()
        {
            var personId = (await GetPersonIdAsync()).ToString();
            //var task = ;

            return await GetObjectAsyncSession(Key.MPPERSONEXT, () => MPPersonClient.Instance.GetPersonExt(personId));
        }

        public async Task<IList<MPFundJob>> GetActiveFundJobsAsync(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();
            //var task = ;

            return await GetObjectAsyncSession(Key.ACTIVE_MP_FUND_JOBS, () => MPFundJobClient.Instance.GetActiveByPersonId(personId));
        }

        public async Task<PersonPCP> GetPersonPCP(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();
            //var task = ;

            return await GetObjectAsyncSession(Key.PERSON_PCP, () => MPPersonClient.Instance.GetPersonPCP(personId));
        }

        public async Task<MPHealthProgram> GetMPHealthProgram(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();
            //var task = ;

            return await GetObjectAsyncSession(Key.MP_HEALTH_PROGRAM, () => MPPersonClient.Instance.GetMPHealthProgram(personId));
        }

        public async Task<MPFundJob> GetPrimaryFundJobAsync(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();

            return await GetObjectAsyncSession(Key.PRIMARY_MP_FUND_JOBS, () => MPFundJobClient.Instance.GetPrimaryByPersonId(personId));
        }
        public async Task<MPPersonFivestar> GetMPPersonFivestarAsync(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();

            return await GetObjectAsyncSession(Key.MPPERSON_FIVESTAR, () => MPPersonFivestarClient.Instance.GetByPersonId(int.Parse(personId)));
        }

        public async Task<PaginatedResult<KnowledgeArticleListItem>> GetMPKnowledgeArticlesByCategoryPersonIdAsync(string categoryId, string personId, string pageSize = "10", string page = "0")
        {
            string key = $"MPKnowledgeArticle_{categoryId}_{personId}_{pageSize}_{page}";
            return await GetVariableObjectAsyncSession(key, () => MPKnowledgeArticleClient.Instance.GetByCategoryPersonId(categoryId, personId, pageSize, page));
        }

        public async Task<List<MedicalProCategory>> GetMedicalProCategoriesAsync(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.MEDICAL_PRO_CATEGORIES, () => MedicalProCategoryClient.Instance.GetByPersonId(personId));
        }
        public async Task<List<MPLocalOffice>> GetLocalOffices(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.LOCAL_OFFICES, () => MPLocalOfficeClient.Instance.GetByPersonId(personId));
        }

        public async Task<List<RefLanguage>> GetRefLanguagesAsync()
        {
            return await GetObjectAsyncSession(Key.REF_LANGUAGES, () => RefLanguageClient.Instance.getAll());
        }

        public async Task<List<MPDisplaySpecialty>> GetDisplaySpecialitiesByUserIdAsync(int userId)
        {
            return await GetObjectAsyncSession(Key.DISPLAY_SPECIALTIES_BY_USER, () => MPProviderAddressClient.Instance.GetDisplaySpecialitiesByUserId(userId));
        }

        public async Task<List<MPDisplaySpecialty>> GetDentistDisplaySpecialties()
        {
            return await GetObjectAsyncSession(Key.DENTIST_DISPLAY_SPECIALTIES, () => MedicalProCategoryClient.Instance.GetDisplaySpecialties("23"));
        }

        public async Task<List<MPDisplaySpecialty>> GetSpecialistDisplaySpecialties()
        {
            return await GetObjectAsyncSession(Key.SPECIALIST_DISPLAY_SPECIALTIES, () => MedicalProCategoryClient.Instance.GetDisplaySpecialties("26"));
        }

        public async Task<UnionContract> GetUnionContract(string personId, string bargUnitId, string billingId)
        {
            return await GetObjectAsyncSession(Key.UNION_CONTRACT, () =>
            UnionContractClient.Instance.Find(personId, bargUnitId, billingId));
        }

        public async Task<List<Document>> GetPersonDocumentsForContract(string personId, DocumentsFilter filter)
        {
            return await GetObjectAsyncSession(Key.PERSON_DOCS_FOR_CONTRACT, () =>
            DocumentClient.Instance.GetPersonDocuments(personId, filter));
        }

        public async Task<List<Document>> GetPersonDocumentsForLettersDashboard(string personId, DocumentsFilter filter)
        {
            return await GetObjectAsyncSession(Key.PERSON_DOCS_FOR_LETTERS_DASHBOARD, () =>
            DocumentClient.Instance.GetPersonDocuments(personId, filter));
        }
        public async Task<List<Document>> GetPersonDocumentsByFilter(string personId, DocumentsFilter filter)
        {
            var filterStr = "no_filter";
            if(filter != null)
            {
                filterStr = JsonConvert.SerializeObject(filter);
            }
            string key = $"FILTERED_DOCUMENTS_{personId}_{filterStr}";
            return await GetVariableObjectAsyncSession(key, () => DocumentClient.Instance.GetPersonDocuments(personId, filter));
        }

        public async Task<List<int>> GetPlansByPersonId(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.PERSON_PLAN_IDS, () => MPEligibiltyClient.Instance.GetPlansByPersonId(int.Parse(personId)));
        }


        public async Task<List<string>> GetPlanCodesByPersonId(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.PERSON_PLAN_CODES, () => MPEligibiltyClient.Instance.GetPlanCodesByPersonId(int.Parse(personId)));
        }

        public async Task<List<MPDependentPerson>> GetDependents(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.DEPENDENTS, () => DependentClient.Instance.GetDependents(int.Parse(personId)));
        }

        public async Task<List<PersonPlanExternalLink>> GetPersonPlanExternalLink(LinkType linkType)
        {
            var personId = await GetPersonIdAsync();
            string key = $"PersonPlanExternalLink_{personId}_{JsonConvert.SerializeObject(linkType)}";
            return await GetVariableObjectAsyncSession(key, () => MPEligibiltyClient.Instance.GetPersonPlanExternalLink(personId, linkType));
        }

        public async Task<MPSSN> GetMPSSN(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.MPSSN, () => MPSSNClient.Instance.GetByPersonId(long.Parse(personId)));
        }

        public async Task<List<Beneficiary>> GetBeneficiariesByPersonId(string personId = null)
        {
            if (personId == null)
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            return await GetObjectAsyncSession(Key.BENEFICIARIES, () => MPSrspPrjClient.Instance.GetBeneficiariesByPersonId(personId));
        }

        public async Task<string> GetDistrictByPersonId(string personId = null)
        {
            personId = (await GetPersonIdAsync()).ToString();
            //var task = ;

            return await GetObjectAsyncSession(Key.PERSON_DISTRICT, () => MPFundJobClient.Instance.GetDistrictByPersonId(personId));
        }

        public void SetPension(List<MPPenPrj> data)
        {
            SetInSession(Key.PENSION, data);
        }
        public async Task<List<MPPenPrj>> GetPensionByPersonIdAsync(string personId)
        {
            //return await MPPenPrjClient.Instance.GetByPersonId(personId);
            if (string.IsNullOrWhiteSpace(personId))
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            ////var task = ;

            return await GetObjectAsyncSession(Key.PENSION, () => MPPenPrjClient.Instance.GetByPersonId(personId));
        }
        public void SetPension401K(MPSrspPrj data)
        {
            if (data != null && data.CurrentBalanceStatus == CommonEntities.Portal.JH.AsyncRequestStatus.READY)
            {
                SetInSession(Key.PENSION401K, data);
                return;
            }

            SetInSessionCacheItem(Key.PENSION401K, new SessionCacheObject<object>() { IsCached = false, Item = data });
        }

        public async Task<MPSrspPrj> GetPension401KByPersonIdAsync(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                personId = (await GetPersonIdAsync()).ToString();
            }
            //var task = ;

            var pension401k = await GetObjectAsyncSession(Key.PENSION401K, () => MPSrspPrjClient.Instance.GetByPersonId(personId));

            if (pension401k == null || pension401k.CurrentBalanceStatus != CommonEntities.Portal.JH.AsyncRequestStatus.READY)
            {
                SetInSessionCacheItem(Key.PENSION401K, new SessionCacheObject<object>() { IsCached = false, Item = pension401k });
            }

            return pension401k;
        }

        public async Task<EntitlementResponse> GetEntitlements()
        {
            var userId = DNNUserId;
            var res = await GetObjectAsyncSession(Key.ENTITLEMENTS, () => EntitlementsClient.Instance.CheckEntitlements(userId, EntitlementTypes.DEFAULT_ELIG_LIST));
            //var res = await EntitlementsClient.Instance.CheckEntitlements(userId, EntitlementTypes.DEFAULT_ELIG_LIST);
            return res;
        }

        public bool CanView(IList<EntitlementPermission> Entitlements, string eligibilityType)
        {

            var resItems = Entitlements.Where(e => e.Entitlement == eligibilityType).ToList();

            foreach (var item in resItems)
            {
                if (item.Permission != EntitlementPermission.PermissionType.None)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CanView(string eligibilityType)
        {
            //If contains at least one not "None" => return true;
            var res = await GetEntitlements();
            if(res == null)
            {
                return false;
            }
            return CanView(res.Entitlements, eligibilityType);
        }

        public async Task<EntitlementPermission> GetEligibilityInfo(string eligibilityType)
        {
            var res = await GetEntitlements();
            var eligInfo = res?.Entitlements?.Where(e => e.Entitlement == eligibilityType)?.FirstOrDefault();
            return eligInfo;
        }

        public DateTime? GetStartDate(IList<EntitlementPermission> Entitlements, string eligibilityType)
        {
            var res = Entitlements.Where(e => e.Entitlement == eligibilityType).FirstOrDefault();
            if (res != null)
            {
                return res.StartDate;
            }

            return null;
        }

        public async Task<DateTime?> GetStartDate(string eligibilityType)
        {
            //If contains at least one not "None" => return true;
            var res = await GetEntitlements();
            return GetStartDate(res.Entitlements, eligibilityType);
        }

        public async Task<string> GetUserAddressOrLocationAsync(params LocationType[] priority)
        {
            var person = await GetMPPersonAsync();
            return await GetUserAddressOrLocationAsync(person, priority);
        }

        public async Task<string> GetUserAddressOrLocationAsync(MPPerson person, params LocationType[] priority)
        {
            if (priority == null || priority.Length == 0)
            {
                priority = new LocationType[] { LocationType.Current, LocationType.Home, LocationType.Work };
            }

            foreach (var item in priority)
            {
                switch (item)
                {
                    case LocationType.Current:
                        if (Request.Cookies["geoLocation_32BJ"] != null)
                        {
                            try
                            {
                                string geoLocation_32BJ = Request.Cookies["geoLocation_32BJ"].Value;
                                dynamic data = JObject.Parse(geoLocation_32BJ);
                                string latitude = data.latitude;
                                string longitude = data.longitude;

                                double lat;
                                double lon;
                                if (double.TryParse(latitude, out lat) && double.TryParse(longitude, out lon))
                                {
                                    return lat.ToString() + "," + lon.ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                Debug.WriteLine(ex.InnerException);
                                break;
                            }
                        }
                        break;
                    case LocationType.Home:
                        if (person != null)
                        {
                            if (person.DisplayAddressLine1 != null || person.DisplayAddressLine2 != null)
                            {
                                var address = HttpUtility.HtmlEncode(person.DisplayAddressLine1 + " " + person.DisplayAddressLine2);
                                return Uri.EscapeDataString(address);
                            }
                            return person.Latitude + "," + person.Longitude;
                        }

                        break;
                    case LocationType.Work:
                        if (person != null)
                        {
                            var primaryJob = await GetPrimaryFundJobAsync(person.PersonId.ToString());

                            if (primaryJob != null)
                            {
                                if (!string.IsNullOrWhiteSpace(primaryJob.DisplayAddress))
                                {
                                    return Uri.EscapeDataString(primaryJob.DisplayAddress);
                                }
                                return primaryJob.WorkLocationLatitude + "," + primaryJob.WorkLocationLongitude;
                            }
                        }
                        break;

                }

            }

            return null;
        }

        public async Task<GeoCoordinate> GetUserLocationAsync(params LocationType[] priority)
        {
            var person = await GetMPPersonAsync();
            return await GetUserLocationAsync(person, priority);
        }
        public async Task<GeoCoordinate> GetUserLocationAsync(MPPerson person, params LocationType[] priority)
        {

            if (priority == null || priority.Length == 0)
            {
                priority = new LocationType[] { LocationType.Current, LocationType.Home, LocationType.Work };
            }

            foreach (var item in priority)
            {
                switch (item)
                {
                    case LocationType.Current:
                        if (Request.Cookies["geoLocation_32BJ"] != null)
                        {
                            try
                            {
                                string geoLocation_32BJ = Request.Cookies["geoLocation_32BJ"].Value;
                                dynamic data = JObject.Parse(geoLocation_32BJ);
                                string latitude = data.latitude;
                                string longitude = data.longitude;

                                double lat;
                                double lon;
                                if (double.TryParse(latitude, out lat) && double.TryParse(longitude, out lon))
                                {
                                    return new GeoCoordinate(lat, lon);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                Debug.WriteLine(ex.InnerException);
                                break;
                            }
                        }
                        break;
                    case LocationType.Home:
                        if (person != null)
                        {
                            return Utilities.GetGeoCoordinate(person.Latitude, person.Longitude);
                        }

                        break;
                    case LocationType.Work:
                        if (person != null)
                        {
                            var primaryJob = await GetPrimaryFundJobAsync(person.PersonId.ToString());

                            if (primaryJob != null)
                            {
                                return Utilities.GetGeoCoordinate(primaryJob.WorkLocationLatitude, primaryJob.WorkLocationLongitude);
                            }
                        }
                        break;

                }

            }

            return null;
        }

        public async Task<CanViewViewModel> CanViewTrainingFund(UserInfo user, PortalSettings portalSettings)
        {
            var sw = Stopwatch.StartNew();
            CanViewViewModel model = null;
            try
            {

                var key = $"StagwellTech.SEIU.CommonDNNEntities.DataProviders.SEIUDNNContext.CanViewTrainingFund({DNNUserId})";
                var cache = Session[key];

                if (cache != null)
                {
                    return (CanViewViewModel)cache;
                }

                var settings = await GetUserSettingsAsync();
                int.TryParse(settings.PersonId, out int personId);

                var baseAddress = TrainingFundHandler.GetBaseAddress();

                var id = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_ID);
                var secret = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_SECRET);
                var subject = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_SUBJECT);

                var token = TrainingFundHandler.GenerateJwtToken(id, subject, secret);

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(20);
                client.BaseAddress = new Uri(baseAddress);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TrainingFundHandler.TOKEN_TYPE, token);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"can-view?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<CanViewViewModel>();
                }

                if (model != null)
                {
                    Session[key] = model;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            sw.Stop();
            Debug.WriteLine("BuildCanViewDict took ===========> " + sw.ElapsedMilliseconds);
            var userName = "unknown";
            if(user != null && user.DisplayName != null)
            {
                userName = user.DisplayName;
            }
            AddEventLog(userName, DNNUserId, "Member Portal", "", EventLogType.ADMIN_ALERT, "CanViewTrainingFund took => " + sw.ElapsedMilliseconds, portalSettings == null ? 0 : portalSettings.PortalId);
            return model;
        }

        public static void AddEventLog(string username, int userId, string portalName, string ip, EventLogType logType, string message, int PortalId)
        {
            try
            {
                //initialize log record
                var objSecurity = new PortalSecurity();
                var log = new LogInfo
                {
                    LogTypeKey = logType.ToString(),
                    LogPortalID = PortalId,
                    LogPortalName = portalName,
                    LogUserName = objSecurity.InputFilter(username, PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoAngleBrackets | PortalSecurity.FilterFlag.NoMarkup),
                    LogUserID = userId
                };
                log.AddProperty("IP", ip);
                log.AddProperty("Message", message);

                //create log record
                var logctr = new LogController();
                logctr.AddLog(log);
            }
            catch (Exception) { }
        }

    }
}
