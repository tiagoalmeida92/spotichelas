using WebGarten2.Html;

namespace UI.Views
{
    // get playlist/new
    internal class NewPlaylistView : HtmlDoc
    {
        public NewPlaylistView() :
            base("Create Playlist",
                 H1(Text("Create Playlist")),
                 Form()
            )
        {
        }


        private static IWritable Form()
        {
            return Form("POST", ResolveUrl.Playlist(), Label("name", "Playlist Name"), InputText("name"),
                        Label("description", "Playlist description"), InputText("description"),
                        InputSubmit("Create"));
        }
    }
}