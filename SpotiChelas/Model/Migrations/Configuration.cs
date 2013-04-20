using System.Collections.ObjectModel;
using Model.Models;

namespace Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.Models.PlaylistDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Model.Models.PlaylistDb context)
        {

            context.Playlists.AddOrUpdate(new Playlist
                {
                    Name = "InitialPlaylist",
                    Description = "Added on Configuration.Seed Method",
                    PlaylistTracks = new Collection<PlaylistTrack>
                        {
                            new PlaylistTrack
                                {
                                    PlaylistId = 1,
                                    SpotifyTrackId = "fake"
                                }
                        }
                });

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
        }
    }
}
