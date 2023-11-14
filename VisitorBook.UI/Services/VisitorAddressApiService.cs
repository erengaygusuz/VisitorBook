
using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.UI.Services
{
    public class VisitorAddressApiService
    {
        private readonly HttpClient _httpClient;

        public VisitorAddressApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VisitorAddressGetResponseDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<VisitorAddressGetResponseDto>>($"visitoraddress");

            return response;
        }

        public async Task<T> GetByIdAsync<T>(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"visitoraddress/{id}");

            return response;
        }

        public async Task<bool> AddAsync(VisitorAddressAddRequestDto visitorAddressAddRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("visitoraddress", visitorAddressAddRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(VisitorAddressUpdateRequestDto visitorAddressUpdateRequestDto)
        {
            var response = await _httpClient.PutAsJsonAsync("visitoraddress", visitorAddressUpdateRequestDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"visitoraddress/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
