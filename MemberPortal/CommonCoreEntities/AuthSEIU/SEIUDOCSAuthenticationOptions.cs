using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUDOCSAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SEIUDOCSAuthenticationOptions()
        {
        }

        public new SEIUDOCSAuthenticationEvents Events
        {
            get { return (SEIUDOCSAuthenticationEvents)base.Events; }

            set { base.Events = value; }
        }
    }
}
