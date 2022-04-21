using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUAuthorizeFilterAttribute : TypeFilterAttribute
    {
        public SEIUAuthorizeFilterAttribute() : base(typeof(SEIUAuthorizeFilter)) { }
    }

    public class SEIUAuthorizeFilter : AuthorizeFilter
    {
        public SEIUAuthorizeFilter(IAuthorizationPolicyProvider provider)
            : base(provider, new[] { new AuthorizeData(Constants.SEIUPolicy) }) { }
    }
}
