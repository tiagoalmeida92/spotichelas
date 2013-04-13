using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using WebGarten2.Html;

namespace UI.Views
{
    internal class PlaylistsView : HtmlDoc
    {
        public PlaylistsView(IEnumerable<Playlist> e)
            : base("Playlists",
                   H1(Text("Playlists")),
                   Ul(e.Select(pl => Li(A(ResolveUrl.Playlist(pl), pl.Name + " - " + pl.Description))).ToArray()),
                   A(ResolveUrl.NewPlaylist(), "Nova PlayList")
                )
        {
        }
    }
}