using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SEIUAuthenticationOptions()
        {
        }

        public new SEIUAuthenticationEvents Events
        {
            get { return (SEIUAuthenticationEvents)base.Events; }

            set { base.Events = value; }
        }
    }
}
