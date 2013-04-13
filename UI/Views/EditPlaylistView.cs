using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using WebGarten2.Html;

namespace UI.Views
{
    internal class EditPlaylistView : HtmlDoc
    {
        public EditPlaylistView(Playlist pl, IEnumerable<Song> songs) :
            base(string.Format("Editar playlist {0}", pl.Name),
                 H1(Text(pl.Name + " - " + pl.Description)),
                 A(ResolveUrl.RemovePlaylist(pl), "Delete playlist"),
                 H2(Text("Add song with spotify id")),
                 AddSong(pl),
                 Ul(
                     songs.Select(
                         song => Li(UpButton(pl, song), DownButton(pl, song), ButtonDelete(pl, song))
                         ).ToArray()
                     )
            )
        {
        }

        private static IWritable UpButton(Playlist pl, Song song)
        {
            return ((HtmlElem) Form("POST", ResolveUrl.MoveUp(pl), Text(song.Artist + " - " + song.Name),
                                    new HtmlElem("input").WithAttr("type", "hidden")
                                                         .WithAttr("name", "SongId")
                                                         .WithAttr("value", song.Id), InputSubmit("Up"))).WithAttr(
                                                             "style", "display:inline;");
        }

        private static IWritable DownButton(Playlist pl, Song song)
        {
            return ((HtmlElem) Form("POST", ResolveUrl.MoveDown(pl),
                                    new HtmlElem("input").WithAttr("type", "hidden")
                                                         .WithAttr("name", "SongId")
                                                         .WithAttr("value", song.Id), InputSubmit("Down"))).WithAttr(
                                                             "style", "display:inline;");
        }

        private static IWritable AddSong(Playlist pl)
        {
            return Form("POST", ResolveUrl.AddSong(pl), Label("SongId", "Spotify Track Id"),
                        InputText("SongId"), InputSubmit("Add"));
        }

        private static IWritable ButtonDelete(Playlist pl, Song song)
        {
            return ((HtmlElem) Form("POST", ResolveUrl.DeleteSong(pl, song),
                                    new HtmlElem("input").WithAttr("type", "hidden")
                                                         .WithAttr("name", "SongId")
                                                         .WithAttr("value", song.Id), InputSubmit("Delete"))).WithAttr(
                                                             "style", "display:inline;");
        }
    }
}