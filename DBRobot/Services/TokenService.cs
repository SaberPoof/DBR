using DBRobot.Interface;
using DBRobot.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace DBRobot.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _tokenEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public TokenService(IConfiguration configuration, Authen authen)
        {
            _tokenEndpoint = configuration["MarketPlaceApi:TokenEndPoint"];
            _clientId = authen.Login;
            _clientSecret = authen.Password;
        }

        public async Task<TokenResponse> GetAccessTokenAsync()
        {
            var request = new ClientCredentialsTokenRequest
            {
                Address = _tokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret
            };

            using var client = new HttpClient();
            var responce = await client.RequestClientCredentialsTokenAsync(request);

            if (responce.IsError)
            {
                throw new Exception($"Ошибка получения токена: {responce.Error}");
            }

            return responce;
        }

     
    }
}
