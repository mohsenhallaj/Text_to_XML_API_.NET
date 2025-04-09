namespace TextToXmlApiNet.Models.Aes
{
    public class AesEncryptionModel
    {
        public string Text { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Iv { get; set; } = string.Empty;
    }
}
