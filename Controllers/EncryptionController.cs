using Microsoft.AspNetCore.Mvc;
using System.Text;
using TextToXmlApiNet.Models.Encryption;
using TextToXmlApiNet.Services.AesImpl;
using TextToXmlApiNet.Services.Rsa;

namespace TextToXmlApiNet.Controllers
{
    /// <summary>
    /// Handles encryption and decryption using AES, RSA, and Base64.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly IAesEncryptionService _aes;
        private readonly IRsaEncryptionService _rsa;

        public EncryptionController(IAesEncryptionService aes, IRsaEncryptionService rsa)
        {
            _aes = aes;
            _rsa = rsa;
        }

        /// <summary>
        /// Encrypts a string using AES, RSA, or BASE64.
        /// </summary>
        [HttpPost("encrypt")]
        [ProducesResponseType(typeof(EncryptionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EncryptionResponse> Encrypt([FromBody] EncryptionRequest request)
        {
            var algo = request.Algorithm?.Trim().ToUpperInvariant();
            Console.WriteLine("Requested Encryption Algorithm: " + algo);

            if (string.IsNullOrWhiteSpace(algo))
                return BadRequest("Algorithm is required.");

            try
            {
                string result = algo switch
                {
                    "AES" => _aes.Encrypt(request.Text),
                    "RSA" => _rsa.Encrypt(request.Text),
                    "BASE64" => Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Text)),
                    _ => throw new ArgumentException($"Unsupported encryption algorithm: '{request.Algorithm}'")
                };

                return Ok(new EncryptionResponse { Encrypted = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption error: " + ex.Message);
                return StatusCode(500, "Encryption failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Decrypts a string using AES, RSA, or BASE64.
        /// </summary>
        [HttpPost("decrypt")]
        [ProducesResponseType(typeof(EncryptionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EncryptionResponse> Decrypt([FromBody] EncryptionRequest request)
        {
            var algo = request.Algorithm?.Trim().ToUpperInvariant();
            Console.WriteLine("Requested Decryption Algorithm: " + algo);

            if (string.IsNullOrWhiteSpace(algo))
                return BadRequest("Algorithm is required.");

            try
            {
                string result = algo switch
                {
                    "AES" => _aes.Decrypt(request.Text),
                    "RSA" => _rsa.Decrypt(request.Text),
                    "BASE64" => Encoding.UTF8.GetString(Convert.FromBase64String(request.Text)),
                    _ => throw new ArgumentException($"Unsupported decryption algorithm: '{request.Algorithm}'")
                };

                return Ok(new EncryptionResponse { Encrypted = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption error: " + ex.Message);
                return StatusCode(500, "Decryption failed: " + ex.Message);
            }
        }
    }
}
