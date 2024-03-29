﻿using VisitorBook.BL.Concrete;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Utilities;
using VisitorBook.DAL.Concrete;

namespace VisitorBook.UI.Extensions
{
    public static class ServiceRepositoryExtension
    {
        public static void AddServiceRepositoryExt(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPropertyMappingService), typeof(PropertyMappingService));
            services.AddScoped(typeof(IFakeDataService), typeof(FakeDataService));
            services.AddScoped(typeof(IUserDataStatisticService), typeof(UserDataStatisticService));
            services.AddScoped(typeof(IUserStatisticService), typeof(UserStatisticService));
            services.AddScoped(typeof(IPlaceStatisticService), typeof(PlaceStatisticService));
            services.AddScoped(typeof(IEmailService), typeof(EmailService));
            services.AddScoped(typeof(IHomeFactStatisticService), typeof(HomeFactStatisticService));
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped(typeof(LocationHelper));
            services.AddScoped(typeof(RazorViewConverter));
        }
    }
}
