using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUDOCSAuthenticationFailedContext : ResultContext<SEIUDOCSAuthenticationOptions>
    {
        public SEIUDOCSAuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            SEIUDOCSAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public Exception Exception { get; set; }
    }
}
