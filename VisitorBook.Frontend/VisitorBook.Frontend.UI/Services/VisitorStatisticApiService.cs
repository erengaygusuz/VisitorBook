using VisitorBook.Frontend.UI.Models.Outputs;

namespace VisitorBook.Frontend.UI.Services
{
    public class VisitorStatisticApiService
    {
        private readonly HttpClient _httpClient;

        public VisitorStatisticApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<HighestCountOfVisitedCityByVisitorOutput> GetHighestCountOfVisitedCityByVisitorAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<HighestCountOfVisitedCityByVisitorOutput>("visitorstatistics/gethighestcountofvisitedcitybyvisitor");

            return response;
        }

        public async Task<HighestCountOfVisitedCountyByVisitorOutput> GetHighestCountOfVisitedCountyByVisitorAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<HighestCountOfVisitedCountyByVisitorOutput>("visitorstatistics/gethighestcountofvisitedcountybyvisitor");

            return response;
        }

        public async Task<LongestDistanceByVisitorOneTimeOutput> GetLongestDistanceByVisitorOneTimeAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<LongestDistanceByVisitorOneTimeOutput>("visitorstatistics/getlongestdistancebyvisitoronetime");

            return response;
        }

        public async Task<LongestDistanceByVisitorAllTimeOutput> GetLongestDistanceByVisitorAllTimeAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<LongestDistanceByVisitorAllTimeOutput>("visitorstatistics/getlongestdistancebyvisitoralltime");

            return response;
        }
    }
}
