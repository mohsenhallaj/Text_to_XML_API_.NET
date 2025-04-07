namespace TextToXmlApiNet.Services.AesImpl
{
    public interface IAesEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
