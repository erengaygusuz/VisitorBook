using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Middlewares
{
    public static class RequestLocalizationCookiesMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLocalizationCookies(this IApplicationBuilder app)
            => app.UseMiddleware<RequestLocalizationCookiesMiddleware>();
    }

    public class RequestLocalizationCookiesMiddleware : IMiddleware
    {
        readonly CookieRequestCultureProvider _provider;

        public RequestLocalizationCookiesMiddleware(IOptions<RequestLocalizationOptions> requestLocalizationOptions)
            => _provider = requestLocalizationOptions.Value.RequestCultureProviders.Where(x => x is CookieRequestCultureProvider).Cast<CookieRequestCultureProvider>().FirstOrDefault();
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_provider != null)
            {
                IRequestCultureFeature feature = context.Features.Get<IRequestCultureFeature>();

                if (feature != null)
                {
                    context.Response.Cookies.Append(_provider.CookieName, CookieRequestCultureProvider.MakeCookieValue(feature.RequestCulture));
                }
            }

            await next(context);
        }
    }
}
