using System;
using System.Diagnostics;

namespace StagwellTech.ServiceBusRPC
{
    internal class DebugLog
    {
        [ConditionalAttribute("DEBUG")]
        internal static void WriteToLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}
