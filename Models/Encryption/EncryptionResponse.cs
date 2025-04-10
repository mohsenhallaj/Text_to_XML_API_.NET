namespace TextToXmlApiNet.Models.Encryption
{
    /// <summary>
    /// Represents the encrypted (or decrypted) result of an operation.
    /// </summary>
    public class EncryptionResponse
    {
        public string Encrypted { get; set; } = string.Empty;
    }
}
