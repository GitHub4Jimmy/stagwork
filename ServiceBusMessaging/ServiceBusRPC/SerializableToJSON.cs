using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.ServiceBusRPC
{
    public class SerializableToJSON<T>
    {
        public string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static T fromJSON(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
