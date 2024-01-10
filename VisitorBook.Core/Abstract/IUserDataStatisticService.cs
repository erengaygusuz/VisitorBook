namespace VisitorBook.Core.Abstract
{
    public interface IUserDataStatisticService
    {
        Task<Tuple<string, string>> GetHighestCountOfVisitedCountyByVisitorAsync();
        Task<Tuple<string, string>> GetHighestCountOfVisitedCityByVisitorAsync();
        Task<Tuple<string, string>> GetHighestCountOfVisitedCountryByVisitorAsync();
        Task<Tuple<string, string>> GetLongestDistanceByVisitorOneTimeAsync();
        Task<Tuple<string, string>> GetLongestDistanceByVisitorAllTimeAsync();
    }
}
