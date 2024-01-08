using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Extensions
{
    public static class LocalizationExtension
    {
        public static void AddLocalizationExt(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization(opts =>
            {
                opts.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(Language).GetTypeInfo().Assembly.FullName!);

                    return factory.Create(nameof(Language), assemblyName.Name!);
                };
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("tr-TR")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
    }
}
