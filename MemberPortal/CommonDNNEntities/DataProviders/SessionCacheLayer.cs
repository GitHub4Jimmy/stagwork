using DotNetNuke.Common.Utilities;
using StagwellTech.SEIU.CommonEntities.DBO.Person.Pension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace StagwellTech.SEIU.CommonDNNEntities.DataProviders
{
    public class SessionCacheLayer
    {
        public class SessionCacheObject<T>
        {
            public T Item { get; set; }
            public bool IsCached { get; set; }

            public SessionCacheObject() {
                IsCached = false;
            }

            public static SessionCacheObject<T> GenerateCacheObject(T item)
            {
                return new SessionCacheObject<T> { Item = item, IsCached = true };
            }

            public static explicit operator SessionCacheObject<T>(SessionCacheObject<object> v)
            {
                if (v == null) return null;
                return GenerateCacheObject((T)v.Item);
            }
        }

        protected HttpRequest Request { get; set; }
        protected HttpSessionState Session { get; set; }
        protected int DNNUserId { get; set; }

        protected enum Key
        {
            USER_SETTINGS,
            MEMBER_DASHBOARD_DATA,
            MPPERSON,
            MPPERSONEXT,
            ACTIVE_MP_FUND_JOBS,
            PERSON_DISTRICT,
            PRIMARY_MP_FUND_JOBS,
            PENSION,
            PENSION401K,
            ENTITLEMENTS,
            CHECK_IF_HAS_UNSEEN,
            GET_DEFAULT_UPCOMMING_EVENTS,
            MEDICAL_PRO_CATEGORIES,
            REF_LANGUAGES,
            DISPLAY_SPECIALTIES_BY_USER,
            DENTIST_DISPLAY_SPECIALTIES,
            SPECIALIST_DISPLAY_SPECIALTIES,
            PERSON_PLAN_IDS,
            UNION_CONTRACT,
            PERSON_DOCS_FOR_CONTRACT,
            LOCAL_OFFICES,
            PERSON_DOCS_FOR_LETTERS_DASHBOARD,
            PERSON_PLAN_CODES,
            PERSON_PCP,
            MP_HEALTH_PROGRAM,
            DEPENDENTS,
            BENEFICIARIES,
            MPSSN,
            MP_ELIGIBILITIES,
            MPPERSON_FIVESTAR,
            OMNI_CHAT_AVAILABILITY
        }

        protected SessionCacheLayer(HttpRequest Request, HttpSessionState Session, int DNNUserId)
        {
            this.Request = Request;
            this.Session = Session;
            this.DNNUserId = DNNUserId;
        }

        protected void Set(Key _key, object value)
        {
            var key = $"{_key}_{DNNUserId}";
            if (Request.RequestContext.HttpContext.Items.Contains(key))
            {
                Request.RequestContext.HttpContext.Items.Remove(key);
            }
            Request.RequestContext.HttpContext.Items.Add(key, value);
        }
        protected SessionCacheObject<object> GetFromSession(Key key)
        {
            //return (SessionCacheObject<object>)Session[$"SessionCacheObject_{key}_{DNNUserId}"];
            return GetVariableFromSession(key.ToString());
        }
        protected SessionCacheObject<object> GetVariableFromSession(string key)
        {
            return (SessionCacheObject<object>)Session[$"SessionCacheObject_{key}_{DNNUserId}"];
        }
        protected void SetInSession(string key, object value)
        {
            SessionCacheObject<object> cacheItem = SessionCacheObject<object>.GenerateCacheObject(value);

            Session.Remove($"SessionCacheObject_{key}_{DNNUserId}");
            Session.Add($"SessionCacheObject_{key}_{DNNUserId}", cacheItem);
        }

        protected void SetInSession(Key key, object value)
        {
            SetInSession(key.ToString(), value);
        }
        protected void SetInSessionCacheItem(Key key, SessionCacheObject<object> cacheItem)
        {
            Session.Remove($"SessionCacheObject_{key}_{DNNUserId}");
            Session.Add($"SessionCacheObject_{key}_{DNNUserId}", cacheItem);
        }

        protected object Get(Key key)
        {
            return Request.RequestContext.HttpContext.Items[$"{key}_{DNNUserId}"];
        }
        protected T GetObject<T>(Key key)
        {
            T value = (T)Get(key);
            return value;
        }

        //protected async Task<T> GetObjectAsync<T>(Key key, Task<T> getter) where T : class
        //{
        //    T value = (T)Get(key);
        //    if (value == null)
        //    {
        //        Debug.WriteLine($"{key} not found. Quering database");
        //        await Task.WhenAll(getter);
        //        value = getter.Result;
        //        Set(key, value);
        //    }
        //    return value;
        //}

        protected async Task<T> GetObjectAsync<T>(Key key, Func<Task<T>> getter) where T : class
        {
            T value = (T)Get(key);
            if (value == null)
            {
                Debug.WriteLine($"{key} not cached. Quering database");
                var job = getter.Invoke();
                await Task.WhenAll(job);
                value = job.Result;
                Set(key, value);
            }
            return value;
        }

        protected async Task<T> GetVariableObjectAsyncSession<T>(string key, Func<Task<T>> getter) where T : class
        {
            SessionCacheObject<T> value = (SessionCacheObject<T>)GetVariableFromSession(key);
            if (value == null || !value.IsCached)
            {
                Debug.WriteLine($"{key} not cached. Quering database");
                var job = getter.Invoke();
                await Task.WhenAll(job);
                SetInSession(key, job.Result);
                return job.Result;
            }
            return value.Item;
        }

        protected async Task<T> GetObjectAsyncSession<T>(Key key, Func<Task<T>> getter) where T : class
        {
            SessionCacheObject<T> value = (SessionCacheObject<T>)GetFromSession(key);
            if (value == null || !value.IsCached || checkIfNeedToRefresh401K(key, value))
            {
                Debug.WriteLine($"{key} not cached. Quering database");
                var job = getter.Invoke();
                await Task.WhenAll(job);
                SetInSession(key, job.Result);
                return job.Result;
            }
            return value.Item;
        }

        private bool checkIfNeedToRefresh401K<T>(Key key, SessionCacheObject<T> value)
        {
            if (key == Key.PENSION401K && typeof(T).Equals(typeof(MPSrspPrj)) && value.Item != null)
            {
                return ((MPSrspPrj)((Object)value.Item)).CurrentBalanceStatus != CommonEntities.Portal.JH.AsyncRequestStatus.READY;
            }
            return false;
        }

        protected async Task<T> SetObjectAsync<T>(Key key, Func<Task<T>> getter, T value = null) where T : class
        {
            if (value == null)
            {
                var job = getter.Invoke();
                await Task.WhenAll(job);
                value = job.Result;
            }
            SetInSession(key, value);
            return value;
        }

    }
}
