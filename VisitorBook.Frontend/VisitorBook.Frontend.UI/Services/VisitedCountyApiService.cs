using VisitorBook.Frontend.UI.Models;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class VisitedCountyApiService
    {
        private readonly HttpClient _httpClient;

        public VisitedCountyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableList<VisitedCountyOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableList<VisitedCountyOutput>>();

            return result;
        }

        public async Task<List<VisitedCountyOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitedCountyOutput>>($"visitedcounties");

            return response;
        }

        public async Task<VisitedCountyOutput> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<VisitedCountyOutput>($"visitedcounties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitedCountyInput visitedCountyAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties", visitedCountyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, VisitedCountyInput visitedCountyAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"visitedcounties/{id}", visitedCountyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitedcounties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
