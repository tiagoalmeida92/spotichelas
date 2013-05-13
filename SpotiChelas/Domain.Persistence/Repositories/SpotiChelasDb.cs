using System.Data.Entity;
using Domain.Entities;

namespace Domain.Persistence.Repositories
{
    public class SpotiChelasDb : DbContext
    {
        public SpotiChelasDb() : base("name= SpotiChelasConnection")
        {
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().HasKey(playlist => playlist.Id);
            modelBuilder.Entity<Playlist>().HasMany(x => x.PlaylistTracks);
            modelBuilder.Entity<PlaylistTrack>().HasKey(track => new {track.PlaylistId, track.SpotifyTrackId});
        }
    }
}