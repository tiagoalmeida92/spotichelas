using System.Collections.Generic;

namespace Dto
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhotoLocation { get; set; }
        public int TotalPlaylists { get; set; }
        public string Email { get; set; }
    }
}