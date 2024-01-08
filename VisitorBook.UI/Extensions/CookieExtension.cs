using Microsoft.AspNetCore.Identity;
using VisitorBook.UI.TokenProviders;

namespace VisitorBook.UI.Extensions
{
    public static class CookieExtension
    {
        public static void AddCookieExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                 opt.TokenLifespan = TimeSpan.FromMinutes(15));

            services.Configure<AccountConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            services.Configure<PasswordResetTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromMinutes(30));

            services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder();
                cookieBuilder.Name = "VisitorBook";

                opt.LoginPath = new PathString("/Auth/Login");
                opt.LogoutPath = new PathString("/App/Profile/Logout");
                opt.AccessDeniedPath = new PathString("/App/Home/AccessDenied");
                opt.Cookie = cookieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.SlidingExpiration = true;
            });
        }
    }
}