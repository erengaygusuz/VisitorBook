using VisitorBook.Frontend.UI.Models;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class CountryApiService
    {
        private readonly HttpClient _httpClient;

        public CountryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableList<CountryOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("countries/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableList<CountryOutput>>();

            return result;
        }

        public async Task<List<CountryOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountryOutput>>($"countries");

            return response;
        }

        public async Task<List<CountryOutput>> GetAllBySubRegionAsync(int subRegionId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountryOutput>>($"counties/getallcountriesbysubregion/{subRegionId}");

            return response;
        }

        public async Task<CountryOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CountryOutput>($"countries/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CountryInput countryAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("countries", countryAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, CountryInput countryAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"countries/{id}", countryAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"countries/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
