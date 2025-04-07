// Path: Services/Rsa/IRsaEncryptionService.cs
using System;

namespace TextToXmlApiNet.Services.Rsa
{
    public interface IRsaEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
