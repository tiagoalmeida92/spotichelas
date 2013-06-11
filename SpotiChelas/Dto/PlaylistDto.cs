using System;
using System.Collections.Generic;

namespace Dto
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int TotalTracks { get; set; }
        public IEnumerable<TrackDto> Tracks { get; set; } 
    }
}