using System.Security.Cryptography;
using System.Text;

namespace DBRobot.Services
{
    public class ProtectService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public ProtectService(string key, string iv)
        {
            if (key.Length != 32 || iv.Length != 16)
            {
                throw new ArgumentException("Ошибка длинны ключ-значения");
            }

            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);
        }

        public string Encrypt(string anyText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using var writer = new StreamWriter(cryptoStream);
                writer.Write(anyText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            var encryptionBytes = Convert.FromBase64String(encryptedText);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream(encryptionBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}