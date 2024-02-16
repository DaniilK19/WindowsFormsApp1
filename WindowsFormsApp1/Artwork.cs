using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ArtistId { get; set; } // Внешний ключ для Artist
        public virtual Artist Artist { get; set; } // Навігаційна властивість
    }
}
