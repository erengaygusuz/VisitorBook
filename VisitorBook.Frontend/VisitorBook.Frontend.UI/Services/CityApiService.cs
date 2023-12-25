using System.Net.Http.Headers;
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

        public async Task<PagedTableListOutput<CityOutput>> GetTableData(DataTablesOptions dataTablesOptions, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("cities/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableListOutput<CityOutput>>();

            return result;
        }

        public async Task<List<CityOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CityOutput>>($"cities");

            return response;
        }

        public async Task<List<CountryOutput>> GetAllByCountryAsync(int countryId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountryOutput>>($"counties/getallcountriesbycountry/{countryId}");

            return response;
        }

        public async Task<CityOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CityOutput>($"cities/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CityInput cityAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("cities", cityAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, CityInput cityAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"cities/{id}", cityAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"cities/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
