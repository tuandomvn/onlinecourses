using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Acme.OnlineCourses.Providers
{
    public class PreferredLanguageCultureProvider : RequestCultureProvider
    {
        private const string CultureCookieName = "Abp.Localization.CultureName";

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // First check if user has explicitly set a language via cookie
            if (httpContext.Request.Cookies.TryGetValue(CultureCookieName, out var cookieCulture))
            {
                // User has explicitly chosen a language - respect that choice
                return new ProviderCultureResult(cookieCulture, cookieCulture);
            }
            
            // Next check if language is specified in query string
            var queryCulture = httpContext.Request.Query["culture"].ToString();
            if (!string.IsNullOrWhiteSpace(queryCulture))
            {
                return new ProviderCultureResult(queryCulture, queryCulture);
            }

            // For new users with no preference set, default to Vietnamese
            return new ProviderCultureResult("vi", "vi");
        }
    }
}