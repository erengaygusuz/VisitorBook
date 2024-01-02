namespace VisitorBook.Core.Abstract
{
    public interface IHomeFactStatisticService
    {
        int GetTotalCountryCount();
        int GetTotalVisitedLocationCount();
        Task<int> GetTotalVisitorCountAsync();
        int GetTotalUserCount();
    }
}
