namespace TextToXmlApiNet.Models.Rsa
{
    /// <summary>
    /// RSA encryption response model.
    /// </summary>
    public class RsaEncryptionResponse
    {
        /// <summary>
        /// The encrypted result, encoded in Base64.
        /// </summary>
        public string Encrypted { get; set; } = string.Empty;
    }
}
