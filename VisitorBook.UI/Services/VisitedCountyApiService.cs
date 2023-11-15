using VisitorBook.Core.Dtos;
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

        public async Task<PagedListDto<VisitedCountyResponseDto>> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<PagedListDto<VisitedCountyResponseDto>>();

            return result;
        }

        public async Task<List<VisitedCountyResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitedCountyResponseDto>>($"visitedcounties");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"visitedcounties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitedCountyRequestDto visitedCountyRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("visitedcounties", visitedCountyRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, VisitedCountyRequestDto visitedCountyRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"visitedcounties/{id}", visitedCountyRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitedcounties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
