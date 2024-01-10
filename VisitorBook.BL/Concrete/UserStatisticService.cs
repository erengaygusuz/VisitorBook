using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Entities;

namespace VisitorBook.BL.Concrete
{
    public class UserStatisticService : IUserStatisticService
    {
        private readonly UserManager<User> _userManager;

        public UserStatisticService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public int GetTotalUserCount()
        {
            return _userManager.Users.Count();
        }

        public async Task<int> GetTotalVisitorUserCountAsync()
        {
            return (await _userManager.GetUsersInRoleAsync(AppRoles.Visitor)).Count;
        }

        public async Task<int> GetTotalVisitorRecorderUserCountAsync()
        {
            return (await _userManager.GetUsersInRoleAsync(AppRoles.VisitorRecorder)).Count;
        }

        public async Task<int> GetTotalAdminUserCountAsync()
        {
            return (await _userManager.GetUsersInRoleAsync(AppRoles.Admin)).Count;
        }
    }
}
