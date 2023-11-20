using VisitorBook.Frontend.UI.Models;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class CityApiService
    {
        private readonly HttpClient _httpClient;

        public CityApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableList<CityOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("cities/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableList<CityOutput>>();

            return result;
        }

        public async Task<List<CityOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CityOutput>>($"cities");

            return response;
        }

        public async Task<CityOutput> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<CityOutput>($"cities/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CityInput cityAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("cities", cityAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, CityInput cityAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"cities/{id}", cityAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"cities/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
