using System;

namespace TextToXmlApiNet.Services.Rsa
{
    public interface IRsaEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);

        /// <summary>
        /// Encrypts using a custom RSA public key (XML format).
        /// </summary>
        string EncryptWithCustomKey(string plainText, string publicKeyXml);
    }
}
