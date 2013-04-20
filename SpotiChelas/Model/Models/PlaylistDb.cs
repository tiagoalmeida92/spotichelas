using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class PlaylistDb : DbContext
    {
        public PlaylistDb() : base("name= DefaultConnection")
        {
            
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().HasKey(playlist => playlist.Id);
            modelBuilder.Entity<PlaylistTrack>().HasKey(track => new {track.PlaylistId, track.SpotifyTrackId});
        }
    }
}
