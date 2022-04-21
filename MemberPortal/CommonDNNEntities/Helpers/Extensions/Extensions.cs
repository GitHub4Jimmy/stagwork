using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Device.Location;
using System.Globalization;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public static class Extensions
    {

        public static void AddCssClass(this HtmlGenericControl element, string cssClass)
        {
            var currentCss = element.Attributes["class"];
            var strCurrentCss = currentCss != null ? currentCss.ToString() : String.Empty;
            strCurrentCss += " " + cssClass;
            element.Attributes["class"] = strCurrentCss;
        }
        public static void RemoveCssClass(this HtmlGenericControl element, string cssClass)
        {
            string currentCss = element.Attributes["class"].ToString();
            currentCss = currentCss.Replace(cssClass, string.Empty);
            element.Attributes["class"] = currentCss;
        }
        public static void SetAttribute(this HtmlGenericControl element, string attribute, string value)
        {
            element.Attributes[attribute] = value;
        }

        public static void AddCssClass(this WebControl element, string cssClass)
        {
            string currentCss = element.Attributes["class"].ToString();
            currentCss += " " + cssClass;
            element.Attributes["class"] = currentCss;
        }
        public static void RemoveCssClass(this WebControl element, string cssClass)
        {
            string currentCss = element.Attributes["class"].ToString();
            currentCss = currentCss.Replace(cssClass, string.Empty);
            element.Attributes["class"] = currentCss;
        }

        public static void SetAttribute(this WebControl element, string attribute, string value)
        {
            element.Attributes[attribute] = value;
        }





        public static void Hide(this System.Web.UI.HtmlControls.HtmlGenericControl element)
        {
            element.Style.Add("display", "none");
        }
        public static void Show(this System.Web.UI.HtmlControls.HtmlGenericControl element)
        {
            element.Style.Remove("display");
        }
        public static void Hide(this System.Web.UI.WebControls.WebControl element)
        {
            element.Style.Add("display", "none");
        }
        public static void Show(this System.Web.UI.WebControls.WebControl element)
        {
            element.Style.Remove("display");
        }

        public static string CapitalizeFirst(this string s)
        {
            bool IsNewSentense = true;
            var result = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (IsNewSentense && char.IsLetter(s[i]))
                {
                    result.Append(char.ToUpper(s[i]));
                    IsNewSentense = false;
                }
                else
                    result.Append(s[i]);

                if (s[i] == '!' || s[i] == '?' || s[i] == '.')
                {
                    IsNewSentense = true;
                }
            }

            return result.ToString();
        }

        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
        public static long? ToNullableLong(this string s)
        {
            long i;
            if (long.TryParse(s, out i)) return i;
            return null;
        }
        public static char? ToNullableChar(this string s)
        {
            char i;
            if (char.TryParse(s, out i)) return i;
            return null;
        }
        public static double? ToNullableDouble(this string s)
        {
            double i;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out i)) return i;
            return null;
        }

        public static bool? ToNullableBool(this string s)
        {
            bool i;
            if (bool.TryParse(s, out i)) return i;
            return null;
        }

        public static List<long> ToListOfLongs(this string str, char delimiter = '|')
        {
            List<long> res = new List<long>();

            if (string.IsNullOrEmpty(str))
            {
                return res;
            }

            string[] sa = str.Split(delimiter);
            foreach (var s in sa)
            {
                long ilong;
                if (long.TryParse(s, out ilong))
                {
                    res.Add(ilong);
                }
            }
            
            return res;
        }

        public static List<int> ToListOfInts(this string str, char delimiter = '|')
        {
            List<int> res = new List<int>();

            if (string.IsNullOrEmpty(str))
            {
                return res;
            }

            string[] sa = str.Split(delimiter);
            foreach (var s in sa)
            {
                int iInt;
                if (int.TryParse(s, out iInt))
                {
                    res.Add(iInt);
                }
            }
            return res;
        }

        public static T ToEnumOrDefault<T>(this string str) where T : struct
        {
            T res;
            if (Enum.TryParse<T>(str, out res))
            {
                return res;
            }
            return default(T);
        }

        public static List<T> ToListOfEnums<T>(this string str, char delimiter = '|') where T : struct
        {
            List<T> res = new List<T>();

            if (string.IsNullOrEmpty(str))
            {
                return res;
            }

            string[] sa = str.Split(delimiter);
            foreach (var s in sa)
            {
                T iT = new T();
                if (Enum.TryParse(s, out iT))
                {
                    res.Add(iT);
                }
            }
            return res;
        }

        public static string ToStringConcat<T>(this IEnumerable<T> list, char separator = '|')
        {
            return separator + string.Join(separator.ToString(), list) + separator;
        }

        public static async Task<string> ConvertToBase64Async(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }


        public static string GetDistanceMiles(this GeoCoordinate from, GeoCoordinate to)
        {
            if (from != null && to != null)
            {
                var distanceInMiles = from.GetDistanceTo(to) / 1609.34f;
                return distanceInMiles.ToString("#,##0.#");
            }

            return null;
        }

        public static bool HasValue<T>(this IEnumerable<T> list)
        {
            if (list != null)
            {
                if (list.Any())
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddPlaceholderOption (this DropDownList ddl, string placeholderValue)
        {
            var li = new ListItem
            {
                Value = placeholderValue,
                Text = placeholderValue,
                Selected = true
            };
            li.Attributes["disabled"] = "disabled";
            li.Attributes["style"] = "display:none";

            ddl.Items.Add(li);
        }

        public static string ToShortDate(this DateTime? date)
        {
            return date.ToString().Split(' ').First();
        }
    }


}
