using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Helpers
{
    public static class TrainingFundSearchHelper
    {
        public const string SEARCH_CONTENT = "search-content";
        public const string FILTER_PRE_APPEND = "filter-";
        public const string PAGE = "page";

        public static PaginationViewModel GetPagination(int totalItems, int itemsPerPage, int tabId, NameValueCollection queryCollection)
        {
            var pagination = new PaginationViewModel();
            var url = DotNetNuke.Common.Globals.NavigateURL(tabId);
            var queryString = GetPaginationQuery(queryCollection);
            var append = queryString.Contains("?") ? "&" : "?";
            var page = GetPage(queryCollection);
            decimal nPages = Decimal.Divide(totalItems, itemsPerPage);
            var totalPages = Math.Ceiling(nPages);

            if (page > 1)
            {
                var previousPage = page - 1;
                pagination.Back = new MPGenericLinkButtonViewModel()
                {
                    Text = "Previous",
                    Link = $"{url}{queryString}{append}{TrainingFundSearchHelper.PAGE}={previousPage}"
                };
            }

            var nextPage = page + 1;
            if (page < totalPages)
            {
                pagination.Next = new MPGenericLinkButtonViewModel()
                {
                    Text = "Next",
                    Link = $"{url}{queryString}{append}{TrainingFundSearchHelper.PAGE}={nextPage}"
                };
            }

            pagination.Links = new List<PaginationLinkViewModel>();
            bool lastIsSpacer = false;
            for (int i = 1; i <= totalPages; i++)
            {
                var maxGap = (page == 1 || page == totalPages) ? 2 : 1;
                var gap = Math.Abs(page - i);

                if (i == 1 || i == totalPages || gap <= maxGap)
                {
                    pagination.Links.Add(new PaginationLinkViewModel()
                    {
                        Text = i.ToString(),
                        Link = $"{url}{queryString}{append}{TrainingFundSearchHelper.PAGE}={i}",
                        isActive = page == i
                    });

                    lastIsSpacer = false;
                }
                else if (gap > maxGap && !lastIsSpacer)
                {
                    pagination.Links.Add(new PaginationLinkViewModel()
                    {
                        Text = "...",
                        isSpacer = true
                    });
                    lastIsSpacer = true;
                }
            }

            return pagination;
        }

        private static string GetPaginationQuery(NameValueCollection queryCollection)
        {
            string query = String.Empty;

            var filter = queryCollection.AllKeys
                .Where(k => k == TrainingFundSearchHelper.SEARCH_CONTENT || k.StartsWith(TrainingFundSearchHelper.FILTER_PRE_APPEND))
                .Select(k => new KeyValuePair<string, string>(k, queryCollection[k]))
                .ToList();

            if (filter.Count > 0)
            {
                query = "?";
                filter.ForEach(kv =>
                {
                    query += $"{kv.Key}={kv.Value}&";
                });
                query = query.TrimEnd('&');
            }

            return query;
        }

        public static int GetPage(NameValueCollection queryCollection)
        {
            string pageStr = queryCollection[TrainingFundSearchHelper.PAGE];
            int page = 1;

            if (!String.IsNullOrEmpty(pageStr))
            {
                Int32.TryParse(pageStr, out page);
            }

            return page;
        }

        public static string GetSearchText(NameValueCollection query)
        {
            var searchContent = query[TrainingFundSearchHelper.SEARCH_CONTENT];
            return searchContent;
        }
        
        public static bool HasFilters(NameValueCollection queryCollection)
        {
            return queryCollection.AllKeys.Any(k => k != null && k.StartsWith(FILTER_PRE_APPEND));
        }

        public static void SetFilterBoxes(NameValueCollection queryCollection,
            List<FilterBoxViewModel> filterBoxViewModels)
        {
            var filterKeys = queryCollection.AllKeys.Where(k => k != null && k.StartsWith(TrainingFundSearchHelper.FILTER_PRE_APPEND)).ToList();

            ClearFilters(filterBoxViewModels);

            if (filterKeys.Count == 0)
            {
                return;
            }

            foreach (var filterKey in filterKeys)
            {
                string header = filterKey.Replace(TrainingFundSearchHelper.FILTER_PRE_APPEND, String.Empty);
                var filterBox = filterBoxViewModels.FirstOrDefault(s => s.DatabaseName == header);

                if (filterBox == null)
                {
                    continue;
                }

                List<string> requestFilters = queryCollection[filterKey].Split(',').ToList();

                foreach (string requestFilter in requestFilters)
                {
                    List<string> values = requestFilter.Split('|').ToList();
                    var filterItems = filterBox.Items;
                    foreach (var value in values)
                    {
                        var filterItem = filterItems?.FirstOrDefault(f => f.Value == value);

                        if (filterItem != null)
                        {
                            filterItem.isSelected = true;
                            filterItems = filterItem.NestedFilters;
                        }
                    }
                }
            }

        }

        private static void ClearFilters(List<FilterBoxViewModel> filters)
        {
            if(filters == null)
            {
                return;
            }

            foreach (var filter in filters)
            {
                if (filter.Items != null)
                {
                    foreach (var item in filter.Items)
                    {
                        item.isSelected = false;
                        if (item.NestedFilters != null && item.NestedFilters.Count > 0)
                        {
                            ClearNestedFilters(item.NestedFilters);
                        }
                    }
                }
            }
        }

        private static void ClearNestedFilters(List<FilterViewModel> itemNestedFilters)
        {
            if (itemNestedFilters != null)
            {
                foreach (var itemNestedFilter in itemNestedFilters)
                {
                    itemNestedFilter.isSelected = false;
                    if (itemNestedFilter.NestedFilters != null && itemNestedFilter.NestedFilters.Count > 0)
                    {
                        ClearNestedFilters(itemNestedFilter.NestedFilters);
                    }
                }
            }
        }

        public static bool HasDefaultFilters(List<FilterBoxViewModel> filters)
        {
            if(filters == null)
            {
                return false;
            }

            foreach (var filter in filters)
            {
                if (filter.Items != null)
                {
                    foreach (var item in filter.Items)
                    {
                        if (item.isSelected || HasNestedFilters(item.NestedFilters))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool HasNestedFilters(List<FilterViewModel> itemNestedFilters)
        {
            if (itemNestedFilters != null)
            {
                foreach (var itemNestedFilter in itemNestedFilters)
                {
                    if (itemNestedFilter.isSelected || HasNestedFilters(itemNestedFilter.NestedFilters))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string BuildSearchQuery(string searchContent, List<FilterBoxViewModel> filters)
        {
            var nameValue = new NameValueCollection();

            if (!String.IsNullOrEmpty(searchContent))
            {
                nameValue.Add(SEARCH_CONTENT, searchContent);
            }

            foreach (var filter in filters)
            {
                var parameterName = FILTER_PRE_APPEND + filter.DatabaseName;
                foreach (var item in filter.Items)
                {
                    if (item.isSelected)
                    {
                        var values = new List<String>();
                        values.Add(item.Value);

                        values.AddRange(GetNestedFilters(item.NestedFilters, item.Value));

                        nameValue.Add(parameterName, String.Join(",", values));
                    }
                }
            }

            return nameValue.ToQueryStringNoEncode();
        }

        private static List<string> GetNestedFilters(List<FilterViewModel> itemNestedFilters, string path)
        {
            var filterPaths = new List<string>();
            if (itemNestedFilters != null)
            {
                foreach (var itemNestedFilter in itemNestedFilters)
                {
                    if (itemNestedFilter.isSelected)
                    {
                        var itemPath = $"{path}|{itemNestedFilter.Value}";
                        filterPaths.Add(itemPath);
                        filterPaths.AddRange(GetNestedFilters(itemNestedFilter.NestedFilters, itemPath));
                    }
                } ;
            }

            return filterPaths;
        }
    }
}
