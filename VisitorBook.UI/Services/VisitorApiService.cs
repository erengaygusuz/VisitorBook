using VisitorBook.Core.Dtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Services
{
    public class VisitorApiService
    {
        private readonly HttpClient _httpClient;

        public VisitorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedListDto<VisitorResponseDto>> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<PagedListDto<VisitorResponseDto>>();

            return result;
        }

        public async Task<List<VisitorResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitorResponseDto>>($"visitors");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"visitors/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitorRequestDto visitorRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors", visitorRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, VisitorRequestDto visitorRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"visitors/{id}", visitorRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitors/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
