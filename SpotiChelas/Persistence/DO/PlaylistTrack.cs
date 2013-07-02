namespace Persistence.DO
{
    public class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public string SpotifyTrackId { get; set; }
        public int Position { get; set; }

        public virtual Playlist Playlist { get; set; }
    }
}