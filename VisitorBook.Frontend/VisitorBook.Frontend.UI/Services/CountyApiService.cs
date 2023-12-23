using VisitorBook.Frontend.UI.Models;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class CountyApiService
    {
        private readonly HttpClient _httpClient;

        public CountyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableList<CountyOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("counties/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableList<CountyOutput>>();

            return result;
        }

        public async Task<List<CountyOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyOutput>>($"counties");

            return response;
        }

        public async Task<List<CountyOutput>> GetAllByCityAsync(int cityId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyOutput>>($"counties/getallcountiesbycity/{cityId}");

            return response;
        }

        public async Task<CountyOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CountyOutput>($"counties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CountyInput countyAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("counties", countyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, CountyInput countyAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"counties/{id}", countyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"counties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
