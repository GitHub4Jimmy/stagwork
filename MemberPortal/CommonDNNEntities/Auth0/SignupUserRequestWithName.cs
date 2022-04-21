using Auth0.AuthenticationApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonDNNEntities.Auth0
{
    public class SignupUserRequestWithName : SignupUserRequest
    {
        //
        // Summary:
        //     The first name of the user (if available).
        //
        // Remarks:
        //     This is the given_name attribute supplied by the underlying API.
        [JsonProperty("given_name")]
        public string FirstName { get; set; }
        //
        // Summary:
        //     The full name of the user (e.g.: John Foo). ALWAYS GENERATED.
        //
        // Remarks:
        //     This is the name attribute supplied by the underlying API.
        [JsonProperty("name")]
        public string FullName { get; set; }
        //
        // Summary:
        //     The last name of the user (if available).
        //
        // Remarks:
        //     This is the family_name attribute supplied by the underlying API.
        [JsonProperty("family_name")]
        public string LastName { get; set; }
    }
}
