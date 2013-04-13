using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using WebGarten2.Html;

namespace UI.Views
{
    internal class PlaylistView : HtmlDoc
    {
        public PlaylistView(Playlist playlist, IEnumerable<Song> songs) :
            base(playlist.Name,
                 H1(Text(playlist.Name + " - " + playlist.Description)),
                 H2(A(ResolveUrl.EditPlaylist(playlist), "Editar playlist")),
                 H2(Text("Musicas")),
                 Ul(
                     songs.Select(
                         song =>
                         Li(
                             Text(string.Format("{0} - {1} {2}:{3}", song.Artist, song.Name, song.Duration.Minutes,
                                                song.Duration.Seconds)))).ToArray())
            )
        {
        }
    }
}