using System.Net.Http;
using UI.Views;
using WebGarten2;
using WebGarten2.Html;

namespace UI.Controllers
{
    internal class HomeController
    {
        [HttpMethod("GET", "/")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
                {
                    Content = new HomeView().AsHtmlContent()
                };
        }
    }
}