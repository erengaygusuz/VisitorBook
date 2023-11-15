using VisitorBook.Core.Dtos;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Services
{
    public class CityApiService
    {
        private readonly HttpClient _httpClient;

        public CityApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedListDto<CityResponseDto>> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("cities/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<PagedListDto<CityResponseDto>>();

            return result;
        }

        public async Task<List<CityResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CityResponseDto>>($"cities");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"cities/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CityRequestDto cityDto)
        {
            var response = await _httpClient.PostAsJsonAsync("cities", cityDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, CityRequestDto cityRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"cities/{id}", cityRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"cities/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
