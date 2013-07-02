using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DO
{
    public class SharedPlaylist
    {
        public string OwnerId { get; set; }
        public int PlaylistId { get; set; }
        public string UserId { get; set; }
        public bool Contributor { get; set; }

        public Playlist Playlist { get; set; }
        public UserProfile Owner { get; set; }
        public UserProfile User { get; set; }
    }
}
