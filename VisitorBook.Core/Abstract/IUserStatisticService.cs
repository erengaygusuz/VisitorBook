namespace VisitorBook.Core.Abstract
{
    public interface IUserStatisticService
    {
        int GetTotalUserCount();
        Task<int> GetTotalVisitorUserCountAsync();
        Task<int> GetTotalVisitorRecorderUserCountAsync();
        Task<int> GetTotalAdminUserCountAsync();
    }
}
