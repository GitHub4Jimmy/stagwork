using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StagwellTech.ServiceBusRPC.Entities
{
    [DataContract(Name = "APIMessageRequest")]
    public class APIMessageRequest : SerializableToJSON<APIMessageRequest>
    {
        [DataMember(Name = "request_id")]
        public Guid RequestId { get; set; }

        [DataMember(Name = "request_name")]
        public string RequestName { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "query_parameters")]
        public IList<APIMessageRequestParameter> QueryParameters { get; set; }

        public string GetParameterValue(string parameterName)
        {
            if (QueryParameters == null || QueryParameters.Count() < 1)
            {
                return null;
            }

            foreach (var parameter in QueryParameters)
            {
                if (parameter.ParameterName.ToUpper() == parameterName.ToUpper())
                {
                    return parameter.ParameterValue;
                }
            }

            return null;
        }

        public List<T> GetParameterAsList<T>(string name)
        {
            var parameterList = GetParameterValue(name);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(parameterList);
        }

    }
}
