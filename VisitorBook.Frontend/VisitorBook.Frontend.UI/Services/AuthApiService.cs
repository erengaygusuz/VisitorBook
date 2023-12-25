using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Models.Outputs;

namespace VisitorBook.Frontend.UI.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RegisterOutput> RegisterAsync(RegisterInput registerInput)
        {
            var response = await _httpClient.PostAsJsonAsync("auths/register", registerInput);

            var result = await response.Content.ReadFromJsonAsync<RegisterOutput>();

            return result;
        }

        public async Task<LoginOutput> LoginAsync(LoginInput loginInput)
        {
            var response = await _httpClient.PostAsJsonAsync("auths/login", loginInput);

            var result = await response.Content.ReadFromJsonAsync<LoginOutput>();

            return result;
        }
    }
}
