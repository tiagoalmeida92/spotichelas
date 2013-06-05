using System;
using System.Collections.Generic;

namespace Persistence.DAO
{
    public class Playlist
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
        public virtual ICollection<PlaylistPermission> Permissions { get; set; }
    }
}