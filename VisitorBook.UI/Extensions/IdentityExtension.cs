using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Entities;
using VisitorBook.DAL.Data;
using VisitorBook.UI.TokenProviders;

namespace VisitorBook.UI.Extensions
{
    public static class IdentityExtension
    {
        public static void AddIdentityExt(this IServiceCollection services)
        {
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "AccountConfirmationTokenProvider";
                options.Tokens.PasswordResetTokenProvider = "PasswordResetTokenProvider";

            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<AccountConfirmationTokenProvider<User>>("AccountConfirmationTokenProvider")
              .AddTokenProvider<PasswordResetTokenProvider<User>>("PasswordResetTokenProvider");
        }
    }
}
