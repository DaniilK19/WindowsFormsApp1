using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WindowsFormsApp1
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Collector> Collectors { get; set; }
    }
}

