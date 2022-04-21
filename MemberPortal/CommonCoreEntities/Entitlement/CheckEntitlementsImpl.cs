using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement
{
    public class CheckEntitlementsImpl
    {
        public EntitlementResponse CheckEntitlements(IServiceProvider serviceProvider, EntitlementRequest eRequest)
        {
            /* otherwise, look through the data table and return all of the relevant permissions */
            EntitlementResponse response = new EntitlementResponse();

            List<EntitlementPermission> responseEntitlements = new List<EntitlementPermission> { };

            Dictionary<string, List<string>> entitlementsByDomain = new Dictionary<string, List<string>>();

            foreach (var entitlement in eRequest.Entitlements)
            {
                var parts = entitlement.Split(".");
                var domain = parts[0];

                if (!entitlementsByDomain.ContainsKey(domain))
                {
                    entitlementsByDomain[domain] = new List<string>();
                }
                entitlementsByDomain[domain].Add(entitlement);
            }


            foreach (var domain in entitlementsByDomain.Keys)
            {
                var entitlements = entitlementsByDomain[domain];

                IEntitlementHandler handler = null;

                try
                {
                    handler = BaseEntitlementHandler.GetHandler(serviceProvider, domain);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (handler != null)
                {
                    try
                    {
                        IList<EntitlementPermission> result = handler.handleRequest(eRequest, entitlements);
                        responseEntitlements.AddRange(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            response.Entitlements = responseEntitlements;

            return response;
        }

        public string CheckEntitlementAsync(IServiceScope scope, string json)
        {
            /* TODO: Complete the logic to verify the Entitlement */
            /* Design questions:  Need to see what's getting passed over as part of the request.  Username?  
             * Do we return everything that they are entitled to? */

            Console.WriteLine("\r\n\nEntitlement Check Triggered...\r\n\n");
            Console.WriteLine(json);

            string errorMsg = "";
            EntitlementRequest eRequest = JsonConvert.DeserializeObject<EntitlementRequest>(json, new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    errorMsg = args.ErrorContext.Error.Message;
                    args.ErrorContext.Handled = true;
                }
            });

            if (errorMsg != "")
            {
                var errorResponse = new EntitlementResponse() { ErrorMessage = errorMsg };
                return errorResponse.toJSON();
            }

            var response = CheckEntitlements(scope.ServiceProvider, eRequest);

            var respStr = response.toJSON();

            Console.WriteLine("==========================================");
            Console.WriteLine(respStr);
            Console.WriteLine("==========================================");

            return respStr;
        }
    }

}
