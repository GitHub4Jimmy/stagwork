using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public class FilterProviderOption
    {
        public string RoutePrefix { get; set; }
        public AuthorizeFilter Filter { get; set; }
    }

    public class AuthenticationFilterProvider
    {
        private readonly FilterProviderOption[] options;

        public AuthenticationFilterProvider(params FilterProviderOption[] options)
        {
            this.options = options;
        }

        public void ProvideFilter(FilterProviderContext context, FilterItem filterItem)
        {
            var route = context.ActionContext.ActionDescriptor.AttributeRouteInfo.Template;

            var filter = options.FirstOrDefault(option => route.StartsWith(option.RoutePrefix))?.Filter;
            if (filter != null)
            {
                if (context.Results.All(r => r.Descriptor.Filter != filter))
                {
                    context.Results.Add(new FilterItem(new FilterDescriptor(filter, (int)FilterScope.Controller)));
                }
            }

            //base.ProvideFilter(context, filterItem);
        }
    }
}
