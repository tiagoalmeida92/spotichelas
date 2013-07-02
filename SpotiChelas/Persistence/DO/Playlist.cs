using System;
using System.Collections.Generic;

namespace Persistence.DO
{
    public class Playlist
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<PlaylistTrack> Tracks { get; set; }
        public virtual ICollection<SharedPlaylist> SharedToUsers { get; set; }
    }
}