
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

        public async Task<VisitorListGetResponseDto> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<VisitorListGetResponseDto>();

            return result;
        }

        public async Task<List<VisitorGetResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitorGetResponseDto>>($"visitors");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"visitors/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitorAddRequestDto visitorAddRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("visitors", visitorAddRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(VisitorUpdateRequestDto visitorUpdateRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync("visitors", visitorUpdateRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitors/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
