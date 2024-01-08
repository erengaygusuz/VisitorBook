namespace VisitorBook.Core.Abstract
{
    public interface IVisitorStatisticService
    {
        Task<Tuple<string, string>> GetHighestCountOfVisitedCountyByVisitorAsync();
        Task<Tuple<string, string>> GetHighestCountOfVisitedCityByVisitorAsync();
        Task<Tuple<string, string>> GetLongestDistanceByVisitorOneTimeAsync();
        Task<Tuple<string, string>> GetLongestDistanceByVisitorAllTimeAsync();
    }
}
