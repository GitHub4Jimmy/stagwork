using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class SEIUAuthenticationEvents
    {
        public Func<SEIUAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        public Func<ValidateTokenContext, IServiceProvider, Task> OnValidateToken { get; set; } = (context, serviceProvider) => Task.CompletedTask;

        public virtual Task AuthenticationFailed(SEIUAuthenticationFailedContext context) => OnAuthenticationFailed(context);

        public virtual Task ValidateToken(ValidateTokenContext context, IServiceProvider serviceProvider) => OnValidateToken(context, serviceProvider);
    }
}
