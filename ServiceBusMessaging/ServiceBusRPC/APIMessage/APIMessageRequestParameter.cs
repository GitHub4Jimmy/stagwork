using System.Runtime.Serialization;

namespace StagwellTech.ServiceBusRPC.Entities
{
    [DataContract(Name = "APIMessageRequestParameter")]
    public class APIMessageRequestParameter : SerializableToJSON<APIMessageRequestParameter>
    {
        [DataMember(Name = "parameter_name")]
        public string ParameterName { get; set; }
        [DataMember(Name = "parameter_value")]
        public string ParameterValue { get; set; }
    }
}
