using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class ValidateTokenContext : ResultContext<SEIUAuthenticationOptions>
    {
        public ValidateTokenContext(
            HttpContext context,
            AuthenticationScheme scheme,
            SEIUAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public string Token { get; set; }
    }
}
