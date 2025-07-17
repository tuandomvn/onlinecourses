using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;

namespace Acme.OnlineCourses.Middlewares
{
    public class DefaultLanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CultureCookieName = "Abp.Localization.CultureName";
        private const string LanguagePreferenceCookie = "Language_Preference_Set";

        public DefaultLanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey(LanguagePreferenceCookie))
            {
                context.Response.Cookies.Append(
                    CultureCookieName,
                    "vi",
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        Path = "/"
                    }
                );

                context.Response.Cookies.Append(
                    LanguagePreferenceCookie,
                    "true",
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        Path = "/"
                    }
                );

                // Update current thread culture for the current request
                var viCulture = new CultureInfo("vi");
                CultureInfo.CurrentCulture = viCulture;
                CultureInfo.CurrentUICulture = viCulture;

                string path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
                if (!path.Contains("/api/") &&
                    !path.EndsWith(".js") &&
                    !path.EndsWith(".css") &&
                    !path.EndsWith(".png") &&
                    !path.EndsWith(".jpg") &&
                    !path.EndsWith(".svg"))
                {
                    if (context.Request.Method == "GET")
                    {
                        string redirectUrl = $"{context.Request.Path}?culture=vi&ui-culture=vi";
                        context.Response.Redirect(redirectUrl);
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

    public static class DefaultLanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseDefaultLanguage(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DefaultLanguageMiddleware>();
        }
    }
}