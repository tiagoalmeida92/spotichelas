using System.Data.Entity;
using Persistence.DAO;

namespace Persistence.Repositories
{
    public class Db : DbContext
    {
        public Db() : base("name= SpotiChelasConnection")
        {
        }


        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<PlaylistPermission> PlaylistPermissions { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                        .HasKey(user => user.UserId)
                        .HasMany(user => user.Playlists);

            modelBuilder.Entity<Playlist>()
                        .HasKey(playlist => new {playlist.Id, playlist.UserId})
                        .HasRequired(playlist => playlist.User)
                        .WithMany(x => x.Playlists)
                        .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<PlaylistTrack>()
                        .HasKey(track => new {track.PlaylistId, track.UserId, track.SpotifyTrackId})
                        .HasRequired(ptrack => ptrack.Playlist)
                        .WithMany(playlist => playlist.PlaylistTracks)
                        .HasForeignKey(track => new {track.PlaylistId, track.UserId})
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlaylistPermission>()
                        .HasKey(permission => new {permission.OwnerId, permission.GrantedUserId, permission.PlaylistId})
                        .HasRequired(p => p.Owner)
                        .WithMany(profile => profile.PlaylistsGivenPermission)
                        .HasForeignKey(permission => permission.OwnerId)
                        .WillCascadeOnDelete(false);
            ;
            modelBuilder.Entity<PlaylistPermission>()
                        .HasRequired(p => p.GrantedUser)
                        .WithMany(profile => profile.PlaylistsTakenPermission)
                        .HasForeignKey(permission => permission.GrantedUserId)
                        .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlaylistPermission>()
                        .HasRequired(p => p.Playlist)
                        .WithMany(playlist => playlist.Permissions)
                        .HasForeignKey(permission => new {permission.PlaylistId, permission.OwnerId})
                        .WillCascadeOnDelete(false);
        }
    }
}