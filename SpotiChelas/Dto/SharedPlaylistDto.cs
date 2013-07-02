using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class SharedPlaylistDto
    {
        public string OwnerId { get; set; }
        public int PlaylistId { get; set; }
        public string UserId { get; set; }
        public bool Contributor { get; set; }
        public string PlaylistName { get; set; }
    }
}
