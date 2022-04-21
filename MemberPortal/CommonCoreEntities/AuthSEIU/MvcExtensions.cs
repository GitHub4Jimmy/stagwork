using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public static class MvcExtensions
    {
        public static IMvcBuilder AddFilterProvider(this IMvcBuilder builder, Func<IServiceProvider, IFilterProvider> provideFilter)
        {
            builder.Services.Replace(
                ServiceDescriptor.Transient(provideFilter));

            return builder;
        }
    }
}
