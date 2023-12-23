using VisitorBook.Frontend.UI.Models;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;
using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Services
{
    public class VisitorApiService
    {
        private readonly HttpClient _httpClient;

        public VisitorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedTableList<VisitorOutput>> GetTableData(DataTablesOptions dataTablesOptions)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors/gettabledata", dataTablesOptions);

            var result = await response.Content.ReadFromJsonAsync<PagedTableList<VisitorOutput>>();

            return result;
        }

        public async Task<List<VisitorOutput>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitorOutput>>($"visitors");

            return response;
        }

        public async Task<VisitorOutput> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<VisitorOutput>($"visitors/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitorInput visitorInput)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors", visitorInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, VisitorInput visitorInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"visitors/{id}", visitorInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"visitors/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
