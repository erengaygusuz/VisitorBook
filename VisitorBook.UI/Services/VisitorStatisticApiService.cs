using VisitorBook.UI.Models;

namespace VisitorBook.UI.Services
{
    public class VisitorStatisticApiService
    {
        private readonly HttpClient _httpClient;

        public VisitorStatisticApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<HighestCountOfVisitedCityByVisitor> GetHighestCountOfVisitedCityByVisitorAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<HighestCountOfVisitedCityByVisitor>("visitorstatistics/gethighestcountofvisitedcitybyvisitor");

            return response;
        }

        public async Task<HighestCountOfVisitedCountyByVisitor> GetHighestCountOfVisitedCountyByVisitorAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<HighestCountOfVisitedCountyByVisitor>("visitorstatistics/gethighestcountofvisitedcountybyvisitor");

            return response;
        }

        public async Task<LongestDistanceByVisitorOneTime> GetLongestDistanceByVisitorOneTimeAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<LongestDistanceByVisitorOneTime>("visitorstatistics/getlongestdistancebyvisitoronetime");

            return response;
        }

        public async Task<LongestDistanceByVisitorAllTime> GetLongestDistanceByVisitorAllTimeAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<LongestDistanceByVisitorAllTime>("visitorstatistics/getlongestdistancebyvisitoralltime");

            return response;
        }
    }
}
