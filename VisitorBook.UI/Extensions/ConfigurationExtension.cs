using VisitorBook.UI.Configurations;

namespace VisitorBook.UI.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurationExt(this IServiceCollection services)
        {
            services.AddScoped(typeof(RegionDataTablesOptions));
            services.AddScoped(typeof(SubRegionDataTablesOptions));
            services.AddScoped(typeof(CountryDataTablesOptions));
            services.AddScoped(typeof(CityDataTablesOptions));
            services.AddScoped(typeof(CountyDataTablesOptions));
            services.AddScoped(typeof(VisitedCountyDataTablesOptions));
            services.AddScoped(typeof(UserDataTablesOptions));
            services.AddScoped(typeof(RoleDataTablesOptions));
            services.AddScoped(typeof(RegisterApplicationDataTablesOptions));
        }
    }
}
