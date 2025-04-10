namespace TextToXmlApiNet.Models.Rsa
{
    /// <summary>
    /// RSA encryption request model.
    /// </summary>
    public class RsaEncryptionRequest
    {
        /// <summary>
        /// The plain text to encrypt.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The public RSA key to use for encryption (optional if server uses its own key).
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;
    }
}
