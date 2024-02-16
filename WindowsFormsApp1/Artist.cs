using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; } // Переконайтеся, що ця властивість присутня

        public byte[] ImageData { get; set; }

        public virtual ICollection<Artwork> Artworks { get; set; } // Відносини one-to-many з Artwork
    }
}
