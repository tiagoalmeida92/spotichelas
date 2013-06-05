using System.Collections.Generic;

namespace Persistence.DAO
{
    public class UserProfile
    {
        //From Membership Framework
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhotoLocation { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<PlaylistPermission> PlaylistsGivenPermission { get; set; }
        public virtual ICollection<PlaylistPermission> PlaylistsTakenPermission { get; set; }
    }
}