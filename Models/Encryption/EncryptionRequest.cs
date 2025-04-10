public class EncryptionRequest
{
    public string Algorithm { get; set; } = "";  // "AES", "RSA", "Base64", etc.
    public string Text { get; set; } = "";
    public string? Key { get; set; }             // Optional for AES, TripleDES, etc.
    public string? PublicKey { get; set; }       // For RSA if needed
}
