using VisitorBook.Core.Dtos;
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

        public async Task<PagedListDto<CountyResponseDto>> GetTableData(DataTablesOptions model)
        {
            var response = await _httpClient.PostAsJsonAsync("counties/gettabledata", model);

            var result = await response.Content.ReadFromJsonAsync<PagedListDto<CountyResponseDto>>();

            return result;
        }

        public async Task<List<CountyResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyResponseDto>>($"counties");

            return response;
        }

        public async Task<List<CountyResponseDto>> GetAllByCityAsync(Guid countyId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CountyResponseDto>>($"counties/getallcountiesbycity/{countyId}");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"counties/{id}");

            return response;
        }

        public async Task<bool> AddAsync(CountyRequestDto countyRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("counties", countyRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, CountyRequestDto countyRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"counties/{id}", countyRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"counties/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
