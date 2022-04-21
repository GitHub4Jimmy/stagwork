using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUDOCSAuthorizeFilterAttribute : TypeFilterAttribute
    {
        public SEIUDOCSAuthorizeFilterAttribute() : base(typeof(SEIUDOCSAuthorizeFilter)) { }
    }

    public class SEIUDOCSAuthorizeFilter : AuthorizeFilter
    {
        public SEIUDOCSAuthorizeFilter(IAuthorizationPolicyProvider provider)
            : base(provider, new[] { new AuthorizeData(Constants.SEIUDocumentsPolicy) }) { }
    }
}
