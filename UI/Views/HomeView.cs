using WebGarten2.Html;

namespace UI.Views
{
    internal class HomeView : HtmlDoc
    {
        public HomeView() :
            base("Home",
                 H1(A(ResolveUrl.Playlists(), "Playlists")),
                 H1(Text("SearchTracks "),
                    Form("POST", ResolveUrl.Search(), InputText("s"), InputSubmit("SearchTracks")))
            )
        {
        }
    }
}