using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TrainingFund.DNN.Integration.Extensions
{
    public static class QueryStringExtensions
    {
        public static string ToQueryString(this NameValueCollection nvc)
        {
            return string.Join("&",
                nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key]))
                    .Select(
                        key => string.Join("&", nvc.GetValues(key).Select(val => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(val))))));
        }
        public static string ToQueryStringNoEncode(this NameValueCollection nvc)
        {
            return string.Join("&",
                nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key]))
                    .Select(
                        key => string.Join("&", nvc.GetValues(key).Select(val => string.Format("{0}={1}", key, val)))));
        }


    }
}
