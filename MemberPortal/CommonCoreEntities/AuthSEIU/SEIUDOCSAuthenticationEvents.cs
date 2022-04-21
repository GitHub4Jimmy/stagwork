using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUDOCSAuthenticationEvents
    {
        public Func<SEIUDOCSAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        public Func<ValidateDOCSTokenContext, IServiceProvider, Task> OnValidateToken { get; set; } = (context, serviceProvider) => Task.CompletedTask;

        public virtual Task AuthenticationFailed(SEIUDOCSAuthenticationFailedContext context) => OnAuthenticationFailed(context);

        public virtual Task ValidateToken(ValidateDOCSTokenContext context, IServiceProvider serviceProvider) => OnValidateToken(context, serviceProvider);
    }
}
