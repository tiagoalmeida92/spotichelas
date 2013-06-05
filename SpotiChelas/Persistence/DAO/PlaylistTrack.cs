namespace Persistence.DAO
{
    public class PlaylistTrack
    {
        public string UserId { get; set; }
        public int PlaylistId { get; set; }
        public string SpotifyTrackId { get; set; }
        public int Order { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}