using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StagwellTech.ServiceBusRPC.Entities
{
    public class SerializeHelper
    {
        public static string SerializeObject(object item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}
