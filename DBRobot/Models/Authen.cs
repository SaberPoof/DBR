namespace DBRobot.Models
{
    public class Authen
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public Authen(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
