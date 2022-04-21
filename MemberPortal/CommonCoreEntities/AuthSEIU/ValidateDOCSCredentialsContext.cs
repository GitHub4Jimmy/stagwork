using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using StagwellTech.SEIU.CommonEntities.Utils.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class ValidateDOCSTokenContext : ResultContext<SEIUDOCSAuthenticationOptions>
    {
        public ValidateDOCSTokenContext(
            HttpContext context,
            AuthenticationScheme scheme,
            SEIUDOCSAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public string Token { get; set; }

        public TokenEncryptionObject Signature { get; set; }
    }
}
