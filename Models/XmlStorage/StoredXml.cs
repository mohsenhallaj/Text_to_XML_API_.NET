using System;
using System.ComponentModel.DataAnnotations;

namespace TextToXmlApiNet.Models.XmlStorage
{
    public class StoredXml
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
