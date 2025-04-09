using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TextToXmlApiNet.Services.Rsa
{
    public class RsaEncryptionService : IRsaEncryptionService
    {
        private readonly RSA _rsa;

        public RsaEncryptionService()
        {
            try
            {
                _rsa = RSA.Create();

                // Load private key from file
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "rsa_private.xml");

                if (!File.Exists(path))
                {
                    Console.WriteLine($"❌ RSA key file not found at: {path}");
                    throw new FileNotFoundException("RSA key file not found.", path);
                }

                string xml = File.ReadAllText(path);
                _rsa.FromXmlString(xml);
                Console.WriteLine("✅ RSA private key loaded from file: " + path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to load RSA key: " + ex.Message);
                throw;
            }
        }

        public string Encrypt(string plainText)
        {
            try
            {
                Console.WriteLine("🔐 Encrypting with RSA (internal key): " + plainText);
                var data = Encoding.UTF8.GetBytes(plainText);
                var encrypted = _rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                var result = Convert.ToBase64String(encrypted);
                Console.WriteLine("✅ RSA Encrypted: " + result);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ RSA Encryption Failed: " + ex.Message);
                throw new Exception("Encryption failed: " + ex.Message);
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                Console.WriteLine("🔓 Decrypting with RSA: " + encryptedText);
                var data = Convert.FromBase64String(encryptedText);
                var decrypted = _rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
                var result = Encoding.UTF8.GetString(decrypted);
                Console.WriteLine("✅ RSA Decrypted: " + result);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ RSA Decryption Failed: " + ex.Message);
                throw new Exception("Decryption failed: " + ex.Message);
            }
        }

        public string EncryptWithCustomKey(string plainText, string publicKeyXml)
        {
            try
            {
                using var customRsa = RSA.Create();
                customRsa.FromXmlString(publicKeyXml);

                var data = Encoding.UTF8.GetBytes(plainText);
                var encrypted = customRsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                var result = Convert.ToBase64String(encrypted);

                Console.WriteLine("✅ RSA Encrypted with custom public key.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ RSA Encryption with custom key failed: " + ex.Message);
                throw new Exception("Encryption with custom key failed: " + ex.Message);
            }
        }
    }
}
