using System.Data.Entity;
using System.Data.Metadata.Edm;
using Persistence.DO;
using System.ComponentModel.DataAnnotations.Schema;

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

            #region User rules

            modelBuilder.Entity<UserProfile>()
                        .HasKey(user => user.UserId)
                        .HasMany(user => user.Playlists);
            #endregion

            #region Playlist rules

            modelBuilder.Entity<Playlist>()
                        .HasKey(playlist => new {playlist.Id, playlist.UserId})
                        .HasRequired(playlist => playlist.User)
                        .WithMany(x => x.Playlists)
                        .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Playlist>()
                        .Property(playlist => playlist.Id)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Playlist>().Property(t => t.Name).HasMaxLength(50);

            #endregion

            #region PlaylistTrack rules

            modelBuilder.Entity<PlaylistTrack>()
                        .HasKey(track => new {track.PlaylistId, track.UserId, track.SpotifyTrackId})
                        .HasRequired(ptrack => ptrack.Playlist)
                        .WithMany(playlist => playlist.Tracks)
                        .HasForeignKey(track => new {track.PlaylistId, track.UserId})
                        .WillCascadeOnDelete(false);

            #endregion

            #region PlaylistPermission rules

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
            #endregion

        }
    }
}