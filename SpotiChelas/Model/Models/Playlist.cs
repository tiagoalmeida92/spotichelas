using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Playlist
    {
        
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }


    }
}
