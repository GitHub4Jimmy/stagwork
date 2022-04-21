using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Extensions
{
    public static class SearchFilterExtensions
    {
        //public static string ToQueryString(this List<SearchFilterViewModel> items, string key)
        //{
        //    string queryString = String.Empty;

        //    for (int i = 0; i < items.Count; i++)
        //    {
        //        var item = items[i];
        //        var name = item.Name;
        //        var values = String.Empty;
        //        if (item.Values != null)
        //        {
        //            for (int j = 0; j < item.Values.Count; j++)
        //            {
        //                values += $"&{key}[{i}].Values[{j}]={item.Values[j]}";
        //            }
        //        }
                
        //        var append = (i != items.Count - 1) ? "&" : "";
        //        queryString = $"{key}[{i}].Name={name}{values}{append}";
        //    }

        //    return queryString;
        //}
    }
}
