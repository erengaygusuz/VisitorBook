using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Services
{
    public class VisitedCountyApiService
    {
        private readonly HttpClient _httpClient;

        public VisitedCountyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VisitedCountyListResponseDto> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<VisitedCountyListResponseDto>();

            return result;
        }

        public async Task<List<VisitedCountyGetResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitedCountyGetResponseDto>>($"visitedcounties");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"visitedcounties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitedCountyAddRequestDto visitedCountyAddRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties", visitedCountyAddRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(VisitedCountyUpdateRequestDto visitedCountyUpdateRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync("visitedcounties", visitedCountyUpdateRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitedcounties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
