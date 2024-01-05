using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace VisitorBook.UI.TokenProviders
{
    public class AccountConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public AccountConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<AccountConfirmationTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
    public class AccountConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }
}
