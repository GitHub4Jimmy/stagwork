using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class AuthorizeData : IAuthorizeData
    {
        public AuthorizeData() { }
        public AuthorizeData(string policy)
        {
            this.Policy = policy;
        }
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}
