using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            #region UserProfile rules

            modelBuilder.Entity<UserProfile>()
                        .HasKey(user => user.UserId)
                        .HasMany(user => user.Playlists);

            modelBuilder.Entity<UserProfile>().HasMany(u => u.SharedPlaylistsByMe);
            modelBuilder.Entity<UserProfile>().HasMany(u => u.SharedPlaylistsToMe);

            #endregion

            #region Playlist rules

            // playlist -> 1 user
            modelBuilder.Entity<Playlist>()
                        .HasKey(playlist => playlist.Id)
                        .HasRequired(playlist => playlist.UserProfile)
                        .WithMany(x => x.Playlists)
                        .HasForeignKey(x => x.UserId)
                        .WillCascadeOnDelete(true);

            // playlist -> N tracks
            modelBuilder.Entity<Playlist>()
                        .HasMany(x => x.Tracks)
                        .WithRequired(x => x.Playlist)
                        .HasForeignKey(x => x.PlaylistId)
                        .WillCascadeOnDelete(true);


            modelBuilder.Entity<Playlist>()
                        .Property(playlist => playlist.Id)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Playlist>()
                        .Property(t => t.Name)
                        .HasMaxLength(30);

            modelBuilder.Entity<Playlist>()
                        .Property(t => t.Description)
                        .HasMaxLength(50);

            #endregion

            #region PlaylistTrack rules

            modelBuilder.Entity<PlaylistTrack>()
                        .HasKey(track => new {track.PlaylistId, track.Position})
                        .HasRequired(ptrack => ptrack.Playlist)
                        .WithMany(playlist => playlist.Tracks)
                        .HasForeignKey(track => track.PlaylistId)
                        .WillCascadeOnDelete(true);

            #endregion

            #region SharedPlaylist rules

            modelBuilder.Entity<SharedPlaylist>()
                        .HasKey(e => new {e.OwnerId, e.PlaylistId, e.UserId})
                        .HasRequired(e => e.Playlist)
                        .WithMany(e => e.SharedToUsers)
                        .HasForeignKey(e => e.PlaylistId);

            modelBuilder.Entity<SharedPlaylist>()
                        .HasRequired(e => e.Owner)
                        .WithMany(e => e.SharedPlaylistsByMe)
                        .HasForeignKey(e => e.OwnerId);

            modelBuilder.Entity<SharedPlaylist>()
                        .HasRequired(e => e.User)
                        .WithMany(e => e.SharedPlaylistsToMe)
                        .HasForeignKey(e => e.UserId);

            #endregion
        }
    }
}