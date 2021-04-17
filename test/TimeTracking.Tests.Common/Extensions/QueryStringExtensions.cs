using System.Linq;
using System.Web;

namespace TimeTracking.Tests.Common.Extensions
{
    public static class QueryStringExtensions
    {
        public static string ToQueryString<T>(this T data)
        {
            var properties = from p in data.GetType().GetProperties()
                             where p.GetValue(data, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null)?.ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}