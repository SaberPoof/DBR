using System.Data;
using DBRobot.Interface;

namespace DBRobot.Services
{
    public class TokenManager
    {
        private readonly ITokenService _tokenService;
        private string _currentToken;
        private DateTime _tokenExpiryTime;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public TokenManager(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> GetValidTokenAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (string.IsNullOrEmpty(_currentToken) || DateTime.UtcNow >= _tokenExpiryTime)
                {
                    Console.WriteLine("Обновление токена");
                    var tokenResponse = await _tokenService.GetAccessTokenAsync();
                    _currentToken = tokenResponse.AccessToken;
                    _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 150);

                }

                return _currentToken;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
