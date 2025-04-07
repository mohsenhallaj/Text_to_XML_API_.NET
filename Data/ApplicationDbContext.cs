using Microsoft.EntityFrameworkCore;
using TextToXmlApiNet.Models.XmlStorage;

namespace TextToXmlApiNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<StoredXml> StoredXmls => Set<StoredXml>();
    }
}
