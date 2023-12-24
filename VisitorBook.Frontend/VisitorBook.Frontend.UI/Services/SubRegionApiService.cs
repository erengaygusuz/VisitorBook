using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class SubRegionApiService
    {
        private readonly HttpClient _httpClient;

        public SubRegionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableListOutput<SubRegionOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("subregions/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableListOutput<SubRegionOutput>>();

            return result;
        }

        public async Task<List<SubRegionOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<SubRegionOutput>>($"subregions");

            return response;
        }

        public async Task<List<SubRegionOutput>> GetAllByRegionAsync(int regionId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<SubRegionOutput>>($"subregions/getallsubregionsbyregion/{regionId}");

            return response;
        }

        public async Task<SubRegionOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<SubRegionOutput>($"subregions/{id}");

            return response;
        }

        public async Task<bool> AddAsync(SubRegionInput subRegionAddInput)
        {
            var response = await _httpClient.PostAsJsonAsync("subregions", subRegionAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, SubRegionInput subRegionAddInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"subregions/{id}", subRegionAddInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"subregions/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
