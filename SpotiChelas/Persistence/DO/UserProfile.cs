using System.Collections.Generic;

namespace Persistence.DO
{
    public class UserProfile
    {
        //From Membership Framework
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhotoLocation { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<SharedPlaylist> SharedPlaylistsByMe { get; set; } 
        public virtual ICollection<SharedPlaylist> SharedPlaylistsToMe { get; set; } 
    }
}