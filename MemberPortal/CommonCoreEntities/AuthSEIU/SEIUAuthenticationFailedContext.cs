using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUAuthenticationFailedContext : ResultContext<SEIUAuthenticationOptions>
    {
        public SEIUAuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            SEIUAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public Exception Exception { get; set; }
    }
}
