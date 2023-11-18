using VisitorBook.UI.Models;
using VisitorBook.UI.Models.Inputs;
using VisitorBook.UI.Models.Outputs;
using VisitorBook.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Services
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

        public async Task<List<CountyOutput>> GetAllByCityAsync(Guid countyId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyOutput>>($"counties/getallcountiesbycity/{countyId}");

            return response;
        }

        public async Task<CountyOutput> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<CountyOutput>($"counties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CountyInput countyAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("counties", countyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, CountyInput countyAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"counties/{id}", countyAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"counties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
