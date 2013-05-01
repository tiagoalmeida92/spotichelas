namespace Domain.Entities
{
    public class PlaylistTrack
    {

        public int PlaylistId { get; set; }
        public string SpotifyTrackId { get; set; }
        public int Order { get; set; }
    }
}