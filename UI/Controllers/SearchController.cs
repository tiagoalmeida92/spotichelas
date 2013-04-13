using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using Common.Entities;
using Services;
using UI.Views;
using WebGarten2;
using WebGarten2.Html;

namespace UI.Controllers
{
    public class SearchController
    {
        private readonly SearchService _searchService = new SearchService();

        [HttpMethod("POST", "/search")]
        public HttpResponseMessage Post(NameValueCollection content)
        {
            string s = content["s"];
            IEnumerable<Song> songResults = _searchService.SearchTracks(s);
            return new HttpResponseMessage
                {
                    Content = new SearchResultsView(songResults).AsHtmlContent()
                };
        }
    }
}