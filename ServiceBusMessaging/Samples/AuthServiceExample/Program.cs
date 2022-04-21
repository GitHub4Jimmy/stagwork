using StagwellTech.ServiceBusRPC;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServiceExample
{
    class Program
    {
        static string ServiceBusConnectionString = "Endpoint=######################################################";

        static void Main(string[] args)
        {

            new ServiceBusRPCService(ServiceBusConnectionString);
            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        [ServiceBusRPCService(queueName: "sample-service")]
        public static string checkForAuthorization(string authRequest)
        {
            Console.WriteLine($"Received auth request - {authRequest}. Returning YES");
            return "YES";
        }
    }
}
