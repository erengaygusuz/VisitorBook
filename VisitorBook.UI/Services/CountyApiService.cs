using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Services
{
    public class CountyApiService
    {
        private readonly HttpClient _httpClient;

        public CountyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CountyListResponseDto> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("counties/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<CountyListResponseDto>();

            return result;
        }

        public async Task<List<CountyGetResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyGetResponseDto>>($"counties");

            return response;
        }

        public async Task<List<CountyGetResponseDto>> GetAllByCityAsync(Guid countyId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyGetResponseDto>>($"counties/getallcountiesbycity/{countyId}");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"counties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CountyAddRequestDto countyAddRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("counties", countyAddRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(CountyUpdateRequestDto countyUpdateRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync("counties", countyUpdateRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"counties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
