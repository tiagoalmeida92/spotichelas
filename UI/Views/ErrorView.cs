using WebGarten2.Html;

namespace UI.Views
{
    internal class ErrorView : HtmlDoc
    {
        public ErrorView(string error) :
            base(error,
                 H1(Text(error)),
                 A(ResolveUrl.Home(), "Home"))
        {
        }
    }
}