using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Playlist
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}