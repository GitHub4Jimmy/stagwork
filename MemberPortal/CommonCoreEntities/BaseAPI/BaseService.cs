using StagwellTech.SEIU.CommonCoreEntities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.ServiceBusRPC;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using System.Diagnostics;
using StagwellTech.SEIU.CommonEntities.Delta;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SirenSDK.Models;
using Microsoft.Extensions.DependencyInjection;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public abstract class BaseService<T> : BaseService<SeiuContext, T>
    {
        protected BaseService(SeiuContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }
    }

    public abstract class BaseService<TContext, T> : BaseService<TContext, T, long> where TContext : DbContext
    {
        protected BaseService(TContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }
    }

    public abstract class BaseService<TContext, T, T_ID> : IService<T, T_ID> where TContext : DbContext
    {
        protected System.IServiceProvider serviceProvider;
        protected TContext _context;
        static string QueueName = "entitlement";
        static string ResponseQueueName = "entitlement-res";


        protected readonly ServiceBusRPCClient rpcClient;

        protected BaseService(TContext context, System.IServiceProvider serviceProvider)
        {
            this._context = context;
            this.serviceProvider = serviceProvider;

            var ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            rpcClient = new ServiceBusRPCClient(ServiceBusConnectionString);

        }

        protected TContext GetContextForMultiThread()
        {
            return serviceProvider.GetService<TContext>();
        }

        public virtual async Task<EntitlementPermission> CheckEntitlement(long userId, string entitlement)
        {
            var response = await CheckEntitlements(userId, new List<string> { entitlement });

            if (response == null || response.Entitlements == null || response.Entitlements.Count < 1)
            {
                return new EntitlementPermission() { Entitlement = entitlement, Permission = EntitlementPermission.PermissionType.None }; /* [ "none", "view", "edit", "admin" ] */
            }

            var permission = response.Entitlements.Where(x => x.Entitlement.ToUpper() == entitlement.ToUpper()).FirstOrDefault();

            if (permission == null)
            {
                return new EntitlementPermission() { Entitlement = entitlement, Permission = EntitlementPermission.PermissionType.None }; /* [ "none", "view", "edit", "admin" ] */
            }

            return permission;
        }

        public virtual async Task<EntitlementResponse> CheckEntitlements(string userId, List<string> entitlements)
        {
            return await CheckEntitlements(long.Parse(userId), entitlements);
        }

        public virtual async Task<EntitlementResponse> CheckEntitlements(long userId, List<string> entitlements)
        {
            EntitlementRequest request = new EntitlementRequest();

            request.UserId = (int)userId;
            request.Entitlements = entitlements;
            request.OnlyEntitled = true;
            
            var response = await rpcClient.rpcRequest(QueueName, ResponseQueueName, request.toJSON());

            string responseStr = Encoding.UTF8.GetString(response.Body);

            Console.WriteLine("responseStr => " + responseStr);

            return EntitlementResponse.fromJSON(responseStr);
        }

        protected BaseService() { }

        public abstract Task<int> GetCount();
        public abstract Task<int> GetFirstOrDefault();

        protected void HandleException(Exception e)
        {
            Debug.WriteLine(e.Message);
            Debug.WriteLine(e.StackTrace);
            throw e;
        }

        protected async Task<TItem> HandleDeltaUpdate<TItem, TDelta>(TItem item, IQueryable<TDelta> deltaDbSet, Expression<Func<TDelta, bool>> filterByEntityId, string source = null)
            where TDelta : class, IDelta<TItem>, TItem, new()
        {
            //Get all delta "NEW", "UPDATE" for this ItemID
            List<TDelta> existingUpdates = null;
            try
            {
                //TODO: Handle case when "DELETE" action is found
                existingUpdates = await deltaDbSet
                    .Where(filterByEntityId)
                    .Where(d => d.DeltaAction == "UPDATE" || d.DeltaAction == "NEW")
                    .ToListAsync();
            }
            catch (Exception exceptionIgnore)
            {
                Debug.WriteLine(exceptionIgnore.Message);
                Debug.WriteLine(exceptionIgnore.StackTrace);
            }


            //Handle delta cases
            if (existingUpdates == null || existingUpdates.Count == 0)
            {
                //Post new if no delta found
                var delta = new TDelta();
                delta.SetID(item);
                delta.LoadFromGeneric(item, "UPDATE");

                if (source != null)
                {
                    delta.DeltaUpdatedBy = source;
                }

                _context.Entry(delta).State = EntityState.Added;
                await _context.SaveChangesAsync();

                return delta;
            }
            else
            {
                var unsentUpdate = existingUpdates
                    .Where(u => u.DeltaSent == null)
                    .FirstOrDefault();

                if (unsentUpdate != null)
                {
                    //Update existing delta, if one exists and wasn't sent yet
                    unsentUpdate.LoadFromGeneric(item, unsentUpdate.DeltaAction);
                    _context.Entry(unsentUpdate).State = EntityState.Modified;

                    if (source != null)
                    {
                        unsentUpdate.DeltaUpdatedBy = source;
                    }

                    await _context.SaveChangesAsync();

                    return unsentUpdate;
                }
                else
                {
                    //Post new delta, if one exists and was already sent
                    var delta = new TDelta();
                    delta.SetID(item);
                    delta.LoadFromGeneric(item, "UPDATE");

                    if (source != null)
                    {
                        delta.DeltaUpdatedBy = source;
                    }

                    _context.Entry(delta).State = EntityState.Added;
                    await _context.SaveChangesAsync();

                    return delta;
                }
            }
        }
    }
}
