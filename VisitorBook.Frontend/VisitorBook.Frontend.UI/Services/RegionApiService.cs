using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class RegionApiService
    {
        private readonly HttpClient _httpClient;

        public RegionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableListOutput<RegionOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("regions/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableListOutput<RegionOutput>>();

            return result;
        }

        public async Task<List<RegionOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<RegionOutput>>($"regions");

            return response;
        }

        public async Task<RegionOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<RegionOutput>($"regions/{id}");

            return response;
        }

        public async Task<bool> AddAsync(RegionInput regionAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("regions", regionAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, RegionInput regionAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"regions/{id}", regionAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"regions/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
