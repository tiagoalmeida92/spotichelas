using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
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
