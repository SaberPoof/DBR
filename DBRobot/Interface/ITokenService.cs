using IdentityModel.Client;

namespace DBRobot.Interface
{
    public interface ITokenService
    {
        Task<TokenResponse> GetAccessTokenAsync();
    }
}
