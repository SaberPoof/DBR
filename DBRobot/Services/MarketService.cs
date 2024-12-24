using System.Text.Json;
using DBRobot.Interface;
using DBRobot.Models;

namespace DBRobot.Services
{
    public class MarketService : IMarketService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenManager _tokenManager;
        private readonly string _apiEndPoint;

        public MarketService(HttpClient httpClient, ITokenManager tokenManager, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenManager = tokenManager;
            _apiEndPoint = configuration["MarketPlaceApi:ApiEndPoint"];
        }

        public async Task<List<Test>> FetchDataAsync()
        {
            var token = await _tokenManager.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(_apiEndPoint);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Test>>(responseBody);

        }

    }
}
