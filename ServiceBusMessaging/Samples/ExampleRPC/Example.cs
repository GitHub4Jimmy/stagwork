using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.DavisVision;
using StagwellTech.SEIU.CommonEntities.DBO.KnowledgeArticles;
using StagwellTech.SEIU.CommonEntities.Document;
using StagwellTech.SEIU.CommonEntities.JohnHancock;
using StagwellTech.SEIU.CommonEntities.Portal.Event;
using StagwellTech.SEIU.CommonEntities.Portal.Registration;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.SendgridService;
using StagwellTech.ServiceBusRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace ExampleRPC
{
    class Example
    {
        const string PERSON_ID = "782602";
        const string MP_PERSON_ID = "127863";
        static void Main(string[] args)
        {
            //var ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            //ServiceBusRPCClient rpcClient = new ServiceBusRPCClient(ServiceBusConnectionString);
            //eventClient = new EventClient(rpcClient);
            //kaClient = new MPKnowledgeArticleClient(rpcClient);
            //GetMPPerson().GetAwaiter().GetResult();
            //LoadMemberDashboard().GetAwaiter().GetResult();
            //PostDocument().GetAwaiter().GetResult();
            //UserSettingsHealthCheck().GetAwaiter().GetResult();
            //UserEmailChange().GetAwaiter().GetResult();
            //PostEvent().GetAwaiter().GetResult();
            //RegistrationFormRequestGet().GetAwaiter().GetResult();
            //GetMPPerson().GetAwaiter().GetResult();
            //PostEvent().GetAwaiter().GetResult();
            //GetEvent().GetAwaiter().GetResult();
            //GetCountries().GetAwaiter().GetResult();
            //UpsertKA().GetAwaiter().GetResult();
            //PutEvent().GetAwaiter().GetResult();
            //DeleteEvent().GetAwaiter().GetResult();
            //RegistrationForm().GetAwaiter().GetResult();
            //ChangeEmailRequest().GetAwaiter().GetResult();
            //ChangePasswordRequest().GetAwaiter().GetResult();
            //DVApi().GetAwaiter().GetResult();
            //JHAPI().GetAwaiter().GetResult();

            //var result = Mail.SendForgotPasswordLink("Test email", "Test email", "mantas.rancevas@stagwell.tech", "url").GetAwaiter().GetResult();
            //var form = RegistrationForm.fromJSON("{\"PersonId\": 597769,\"Email\": \"mantas.rancevas + 597769@stagwell.tech\"}");

            //var person = await MPPersonClient.Instance.Get("PERSON_ID");

            //FixAuth0UserNames.DoStuf().GetAwaiter().GetResult();

            var helper = new SessionHelper();

            helper.createSessions();

        }


        static async Task UserSettingsHealthCheck()
        {
            var currentName = "UserSettingsHealthCheck";
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"============ {currentName} ===============");

            var success = await UserSettingsClient.Instance.LoadMemberDashboard(139);

            List<long> times = new List<long>();
            for (int i = 0; i < 100; i++)
            {

                long millisecondsStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var obj = await UserSettingsClient.Instance.LoadMemberDashboard(139);

                long millisecondsEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                long took = millisecondsEnd - millisecondsStart;
                times.Add(took);
                Console.WriteLine($"============ {took} ===============");
            }
            var average = times.Average();
            Console.WriteLine($"Average ============ {average} ===============");
            //Enumerable.Average()
            //Console.WriteLine($"Deleted status: {obj.FirstName} {obj.LastName}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }

        static async Task UserEmailChange()
        {
            var success = await UsersClient.Instance.RequestPasswordReset("597780");

            if (success)
            {
                Console.WriteLine("success");
            }
        }

        static async Task PostDocument()
        {
            PortalDocument doc = new PortalDocument() {
                Id = Guid.NewGuid().ToString(),
                PlanCode = "A",
                ContractId = null,
                PersonId = 3988787,
                CategoryName = "General",
                DocumentType = "Form",
                ContentType = null,
                DisplayName = "Enroll a Dependent Test",
                FileName = "enroll_a_dependent_form.pdf",
                FileLocation = "/v3doc/general/enroll_a_dependent_form.pdf",
                DocEffectiveDate = new DateTime(2020, 10, 1)
            };
            var success = await DocumentClient.Instance.PostPortalDocument(doc);

            if (success != null)
            {
                Console.WriteLine("success");
            } else
            {
                Console.WriteLine("got null");
            }
        }

        static async Task GetMPPerson()
        {
            var currentName = "GetMPPerson";
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"============ {currentName} ===============");
            for (int i = 0; i < 100; i++)
            {

                long millisecondsStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var obj = await MPPersonClient.Instance.Get("3962479");
                long millisecondsEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Console.WriteLine($"============ {millisecondsEnd - millisecondsStart} ===============");
            }
            //Console.WriteLine($"Deleted status: {obj.FirstName} {obj.LastName}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }

        static async Task LoadMemberDashboard()
        {
            var currentName = "LoadMemberDashboard";
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"============ {currentName} ===============");
            for (int i = 0; i < 100; i++)
            {

                long millisecondsStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var obj = await UserSettingsClient.Instance.LoadMemberDashboard(139);
                long millisecondsEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Console.WriteLine($"============ {millisecondsEnd - millisecondsStart} ===============");
            }
            //Console.WriteLine($"Deleted status: {obj.FirstName} {obj.LastName}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }

        static async Task GetCountries()
        {
            var currentName = "GetCountries";
            var sw = Stopwatch.StartNew();
            var objs = await CountryClient.Instance.getAll();
            Console.WriteLine($"============ {currentName} ===============");
            foreach (var item in objs)
            {
                Console.WriteLine($"{item.CountryName}");
            }
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task GetRSVPS()
        {
            var currentName = "GetRSVPS";
            var sw = Stopwatch.StartNew();
            var objs = await EventClient.Instance.GetRSVPs("BADFB899-1ABB-4EE5-817B-08D7D56FBC92");
            Console.WriteLine($"============ {currentName} ===============");
            foreach (var item in objs)
            {
                Console.WriteLine("success");
            }
        }

        static async Task JHAPI()
        {
            var planSummarys = JHApiClient.Instance.GetParticipantPlanSummary("043643191");
            var planSummarys2 = JHApiClient.Instance.GetParticipantPlans("043643191");
        }
        static async Task DVApi()
        {
            var response = DVApiClient.Instance.GetMemberEligibility("078964369", "Abraham", "Smith", "1971-03-20");
        }

        static async Task UpdateMpPerson()
        {
            var ssn3 = await MPPersonClient.Instance.UpdateMobilePhone("597769", "+370123456789");

        }

        static async Task GetRSVPS2()
        {
            var ssn = await MPSSNClient.Instance.GetByPersonId(732435);

            var newSSN = new MPSSN() {
                PersonId = 1234,
                SSNFull = "123456789"
            };
            await MPSSNClient.Instance.Post(newSSN);


            ssn.SSNFull = "123456789";

            await MPSSNClient.Instance.Put(ssn);
            //var currentName = "GetRSVPS";
            //var sw = Stopwatch.StartNew();
            //var objs = await eventClient.GetRSVPs("BADFB899-1ABB-4EE5-817B-08D7D56FBC92");
            //Console.WriteLine($"============ {currentName} ===============");
            //foreach (var item in objs)
            //{
            //    Console.WriteLine($"{item.PersonId}");
            //}
            //sw.Stop();
            //Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task RegistrationFormRequest()
        {

            var form = new RegistrationForm() {
                ID = Guid.NewGuid(),
                PersonId = 597768,
                Email = "mantas.rancevas+597768@stagwell.tech"
            };

            await RegistrationFormClient.Instance.CreateFromCRM(form);


            var result = await RegistrationFormClient.Instance.Get(form.ID);
            //var currentName = "GetRSVPS";
            //var sw = Stopwatch.StartNew();
            //var objs = await eventClient.GetRSVPs("BADFB899-1ABB-4EE5-817B-08D7D56FBC92");
            //Console.WriteLine($"============ {currentName} ===============");
            //foreach (var item in objs)
            //{
            //    Console.WriteLine($"{item.PersonId}");
            //}
            //sw.Stop();
            //Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task RegistrationFormRequestGet()
        {

            for (int i = 0; i < 100; i++)
            {
                long millisecondsStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var result = await RegistrationFormClient.Instance.Get(new Guid("5EBFCEC6-B71C-4CA3-EF2F-08D8270B1649"));
                long millisecondsEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Console.WriteLine($"============ {millisecondsEnd - millisecondsStart} ===============");
            }
            //Console.WriteLine(result.toJSON());

            //var currentName = "GetRSVPS";
            //var sw = Stopwatch.StartNew();
            //var objs = await eventClient.GetRSVPs("BADFB899-1ABB-4EE5-817B-08D7D56FBC92");
            //Console.WriteLine($"============ {currentName} ===============");
            //foreach (var item in objs)
            //{
            //    Console.WriteLine($"{item.PersonId}");
            //}
            //sw.Stop();
            //Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task ChangeEmailRequest()
        {
            await UsersClient.Instance.ChangeEmailByPersonId("597767", "mantas.rancevas+597767+@stagwell.tech");
        }
        static async Task ChangePasswordRequest()
        {
            var result = await UsersClient.Instance.RequestPasswordReset("597767");
        }
        static async Task CountriesLoad()
        {
            var result = await CountryClient.Instance.getAll();
        }
        static async Task DeleteEvent()
        {
            var currentName = "PutEvent";
            var sw = Stopwatch.StartNew();
            var status = await EventClient.Instance.DeleteEvent("3B1F3D0E-C1AE-4938-AACF-093896F71A87");
            Console.WriteLine($"============ {currentName} ===============");
            Console.WriteLine($"Deleted status: {status}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task GetEvent()
        {
            var currentName = "GetEvent";
            var sw = Stopwatch.StartNew();
            var data = await EventClient.Instance.GetUpcoming<Event>("3962479", new StagwellTech.SEIU.CommonEntities.Filters.Event.EventFilter
            {
                LimitDays = 360
            });
            
            Console.WriteLine($"============ {currentName} ===============");
            foreach (var item in data)
            {
                Console.WriteLine($"Update status: {item.EventId}");
            }
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task PutEvent()
        {
            var currentName = "PutEvent";
            var sw = Stopwatch.StartNew();
            Event myEvent = new Event
            {
                Title = "This is a test event",
                Description = "Added a description to this event",
                StartDate = DateTime.Now,
                EventId = "8351B05F-8363-48DC-8C47-D04988C51D6A"
            };
            var status = await EventClient.Instance.UpdateEvent(myEvent);
            Console.WriteLine($"============ {currentName} ===============");
            Console.WriteLine($"Update status: {status}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task PostEvent()
        {
            var currentName = "PostEvent";
            var sw = Stopwatch.StartNew();
            Event myEvent = new Event
            {
                EventId = Guid.NewGuid().ToString(),
                Title = "This is a test event - Eddited on upsert AAAAAAAAAAAAAAA",
                Description = "Another edit. SB. And another. And final update",
                StartDate = DateTime.Now,
                StatusId = 100000003,
                StatusName = "Live"
            };
            var postedEvent = await EventClient.Instance.UpsertEvent(myEvent);
            Console.WriteLine($"============ {currentName} ===============");
            Console.WriteLine($"{postedEvent.EventId}");
            Console.WriteLine($"{postedEvent.toJSON()}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }
        static async Task UpsertKA()
        {
            var currentName = "UpsertKA";
            var sw = Stopwatch.StartNew();
            MPKnowledgeArticle ka = new MPKnowledgeArticle
            {
                ArticleGuid = Guid.Parse("1F60CEB4-5D8A-BA11-A812-000D3A8A4127"),
                ArticleCode = "KA-PROTAL-0000",
                Title = "Test Article Editted",
                Description = "Test Article",
                Keywords = "Test Article",
                Content = "Test Article",
                CategoryId = "5",
                CategoryName = "Dental",
                PlanCode = "MPD4",
                PlanName = "Delta Dental PPO+NYSELECT",
                StatusId = 7,
                StatusName = "Published",
                ModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                LanguageId = 1033,
                LanguageCode = "en-us",
                LanguageName = "English - United States",
                Score = 7.7F
            };
            var postedEvent = await MPKnowledgeArticleClient.Instance.Upsert(ka);
            Console.WriteLine($"============ {currentName} ===============");
            Console.WriteLine($"{postedEvent.ArticleGuid}");
            Console.WriteLine($"{postedEvent.toJSON()}");
            sw.Stop();
            Console.WriteLine($"=================={currentName} {sw.ElapsedMilliseconds.ToString("# ##0")} ms =================");
        }

        static async Task MainAsync()
        {
        }
    }
}