using System.Net;

namespace DBRobot.Services
{
    public class AuthenticationManager
    {
        private readonly string _filePath;
        private readonly ProtectService _protectService;

        public AuthenticationManager(string filePath, ProtectService protectService)
        {
            _filePath = filePath;
            _protectService = protectService;
        }

        public (string Login, string Password) GetAuthenticationData()
        {

            if (File.Exists(_filePath))
            {
                var encryptedData = File.ReadAllText(_filePath);
                var decryptedData = _protectService.Decrypt(encryptedData);
                var credentials = decryptedData.Split(';');
                return (credentials[0], credentials[1]);
            }
            else
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                string login;
                string password;

                do
                {
                    Console.Write("Введите логин: ");
                    login = Console.ReadLine();

                    if (string.IsNullOrEmpty(login))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Логин не может быть пустым.");
                        Console.ForegroundColor = originalColor;
                    }
                } while (string.IsNullOrEmpty(login));

                while (true)
                {
                    Console.Write("Введите пароль: ");
                    var firstPassword = ReadPassword();

                    Console.Write("Повторите пароль: ");
                    var secondPassword = ReadPassword();

                    if (firstPassword == secondPassword)
                    {
                        password = firstPassword;
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Пароль не совпадают. Попробуйте снова.");
                        Console.ForegroundColor = originalColor;
                    }
                }

                var authenticationData = $"{login}:{password}";
                var encryptedData = _protectService.Encrypt(authenticationData);
                File.WriteAllText(_filePath, encryptedData);

                return (login, password);
            }
        }

        private string ReadPassword()
        {
            var password = string.Empty;
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return password;
        }
    }
}
