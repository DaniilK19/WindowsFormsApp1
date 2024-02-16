using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WindowsFormsApp1
{
    public class GalleryContextFactory : IDesignTimeDbContextFactory<GalleryContext>
    {
        public GalleryContext CreateDbContext(string[] args)
        {
            // Отримайте шлях до каталогу проекту
            var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}WindowsFormsApp1", Path.DirectorySeparatorChar);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<GalleryContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new GalleryContext(builder.Options);
        }
    }
}
