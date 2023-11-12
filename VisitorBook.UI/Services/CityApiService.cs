using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
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

        public async Task<CityListResponseDto> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("cities/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<CityListResponseDto>();

            return result;
        }

        public async Task<List<CityGetResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CityGetResponseDto>>($"cities");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"cities/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CityAddRequestDto cityAddRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("cities", cityAddRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(CityUpdateRequestDto cityUpdateRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync("cities", cityUpdateRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"cities/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
