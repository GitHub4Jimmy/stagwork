using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StagwellTech.ServiceBusRPC;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.BusClients;

namespace ServiceBusRPC
{    
    class EntitlementServiceTester
    {
        static int failCount = 0;

        static void Main(string[] args)
        {
            try
            {
                MainAsync().GetAwaiter().GetResult();
            }
            catch (NullReferenceException)
            {
                failCount++;
                Console.WriteLine($"");
                Console.WriteLine($"FAILED. failCount: {failCount}. Trying again");
                Console.WriteLine($"");
                Main(null);
            }
        }

        static async Task MainAsync()
        {
            long totalSpan = 0;
            long maxSpan = long.MinValue;
            long minSpan = long.MaxValue;
            int numOfIterations = 100;
            List<long> allspans = new List<long>();

            

            await CreateEntitlementRequestEligibility2();

            for (int i = 0; i < numOfIterations; i++)
            {
                long sendingAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var response = await CreateEntitlementRequestEligibility2();

                long responseAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                totalSpan = totalSpan + (responseAt - sendingAt);
                allspans.Add(responseAt - sendingAt);

                if (responseAt - sendingAt > maxSpan)
                    maxSpan = responseAt - sendingAt;

                if (responseAt - sendingAt < minSpan)
                    minSpan = responseAt - sendingAt;
                var responsMessage = response.toJSON();
                Console.WriteLine($"\r\n====================" +
                    $"              \r\nReceived Response:\r\n====================\r\nSequenceNumber:{i} \nBody: {responsMessage}" +
                    $"              \r\nTook {responseAt - sendingAt} ms\r\n");
                Console.WriteLine("\r\n\nEntitlement check took => " + (responseAt - sendingAt) + "\r\n\n");
            }

            Console.WriteLine($"\r\n\n\n\nAfter {numOfIterations} tries... on average it took {totalSpan / numOfIterations} ms. Minimum: {minSpan} ms. Maximum: {maxSpan} ms.");

            Console.ReadKey();
        }

        public async static Task<EntitlementResponse> CreateEntitlementRequestEligibility2()
        {

            var data = await UserSettingsClient.Instance.LoadMemberDashboard(USER_IDS[RANDOM.Next(USER_IDS.Length)]);
            return data?.Entitlements;
        }
        public async static Task<EntitlementResponse> CreateEntitlementRequestEligibility()
        {

               var client = EntitlementsClient.Instance;
            return await client.GetUserEntitlements(USER_IDS[RANDOM.Next(USER_IDS.Length)], EntitlementTypes.DEFAULT_ELIG_LIST);
        }

        public static string FormatEntitlementResponse(string response)
        {
            return response;
        }
        public static Random RANDOM = new Random();
        public static int[] USER_IDS = new int[]
        {
            12,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            56,
            57,
            74,
            75,
            76,
            77,
            78,
            79,
            85,
            86,
            87,
            88,
            89,
            90,
            91,
            92,
            93,
            94,
            95,
            96,
            97,
            98,
            99,
            100,
            101,
            102,
            110,
            111,
            112,
            113,
            114,
            115,
            116,
            117,
            118,
            119,
            120,
            121,
            122,
            123,
            124,
            125,
            126,
            127,
            128,
            129,
            130,
            131,
            132,
            133,
            134,
            135,
            136,
            137,
            138,
            139,
            140,
            142,
            143,
            144,
            145,
            146,
            147,
            148,
            149,
            150,
            151,
            159,
            160,
            161,
            162,
            163,
            164,
            165,
            166,
            167,
            168,
            169,
            170,
            172,
            173,
            174,
            175,
            176,
            177,
            178,
            179,
            180,
            181,
            182,
            183,
            184,
            187,
            189,
            190,
            191,
            192,
            193,
            195,
            196,
            197,
            198,
            199,
            200,
            201,
            202,
            203,
            204,
            205,
            206,
            207,
            208,
            209,
            210,
            211,
            212,
            213,
            214,
            215,
            216,
            217,
            218,
            219,
            220,
            221,
            222,
            223,
            224
        };
    }
}
