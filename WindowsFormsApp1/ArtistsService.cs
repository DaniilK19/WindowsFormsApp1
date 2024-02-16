using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ArtistsService
    {
        private readonly GalleryContext _context;

        public ArtistsService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GalleryContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GalleryDb;Trusted_Connection=True;");


            _context = new GalleryContext(optionsBuilder.Options);
        }

        public List<Artist> GetArtists()
        {
            return _context.Artists.ToList();
        }

        public Artist GetArtistById(int id)
        {
            return _context.Artists.FirstOrDefault(artist => artist.ArtistId == id);
        }
        public void ClearAllArtists()
        {
            // WARNING: This will delete all records from the artists table in the database!
            var allArtists = _context.Artists.ToList();
            foreach (var artist in allArtists)
            {
                _context.Artists.Remove(artist);
            }
            _context.SaveChanges();
        }
        public void AddArtist(Artist artist)
        {
            if (artist != null)
            {
                _context.Artists.Add(artist);
                _context.SaveChanges();
            }
        }

        public void UpdateArtist(Artist artist)
        {
            if (artist.ArtistId == 0)
            {
                _context.Artists.Add(artist);
            }
            else
            {
                var existingArtist = _context.Artists.Find(artist.ArtistId);
                if (existingArtist != null)
                {
                    existingArtist.Name = artist.Name;
                    existingArtist.Bio = artist.Bio;
                    existingArtist.ImageData = artist.ImageData;
                    _context.Artists.Update(existingArtist);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteArtist(int artistId)
        {
            var artist = _context.Artists.Find(artistId);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                _context.SaveChanges();
            }
        }
    }
}
