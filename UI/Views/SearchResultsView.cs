using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using WebGarten2.Html;

namespace UI.Views
{
    internal class SearchResultsView : HtmlDoc
    {
        public SearchResultsView(IEnumerable<Song> songs) :
            base("Resultados da pesquisa",
                 A(ResolveUrl.Home(), "Home"),
                 H1(Text("Musicas: resultados " + songs.Count())),
                 SongList(songs)
            // H1(Text("Musicas: resultados " + songs.Count())),
            //ArtistList(artists),
            //H1(Text("Musicas: resultados " + songs.Count())),
            //AlbumsList(artists)
            )
        {
        }


        /* private static IWritable AlbumsList(IEnumerable<Artist> artists)
        {
            throw new NotImplementedException();
        }

        private static IWritable ArtistList(IEnumerable<Artist> artists)
        {
            throw new NotImplementedException();
        }
        */

        private static IWritable SongList(IEnumerable<Song> songs)
        {
            return
                Ul(
                    songs.Select(
                        song =>
                        Li(Text(string.Format("{0} - {1} {2}:{3} {4}", song.Artist, song.Name, song.Duration.Minutes,
                                              song.Duration.Seconds, song.Id))))
                         .ToArray());
        }
    }
}