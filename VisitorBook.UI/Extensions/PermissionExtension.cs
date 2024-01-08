using Microsoft.AspNetCore.Authorization;
using VisitorBook.UI.Filters;

namespace VisitorBook.UI.Extensions
{
    public static class PermissionExtension
    {
        public static void AddPermissionExt(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}
