namespace VisitorBook.UI.Extensions
{
    public static class CookieExtension
    {
        public static void AddCookieExt(this IServiceCollection services)
        {
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