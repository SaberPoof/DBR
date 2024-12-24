namespace DBRobot.Interface
{
    public interface ITokenManager
    {
        Task<string> GetTokenAsync();
    }
}
