using System;
using System.Security.Cryptography;
using System.Text;

namespace TextToXmlApiNet.Services.AesImpl
{
    public class AesEncryptionService : IAesEncryptionService
    {
        // Use a fixed 256-bit key and IV (for AES-256)
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");
        private static readonly byte[] _iv  = Encoding.UTF8.GetBytes("abcdef9876543210");

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText);
            var encrypted = encryptor.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor();
            var buffer = Convert.FromBase64String(encryptedText);
            var decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
