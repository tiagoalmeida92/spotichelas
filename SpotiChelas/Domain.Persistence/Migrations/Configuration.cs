using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using Domain.Entities;
using Domain.Persistence.Repositories;

namespace Domain.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<PlaylistDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PlaylistDb context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Playlists.AddOrUpdate(new Playlist
                {
                    Name = "InitialPlaylist",
                    Description = "Added on Configuration.Seed Method",
                    PlaylistTracks = new Collection<PlaylistTrack>
                        {
                            new PlaylistTrack
                                {
                                    PlaylistId = 1,
                                    SpotifyTrackId = "5jROXt1Iz8Zk98WobPtC4k"
                                }
                        }
                });

            
        }
    }
}