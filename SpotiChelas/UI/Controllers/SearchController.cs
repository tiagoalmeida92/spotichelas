using System.Web.Mvc;
using Services;

namespace UI.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService, IPlaylistService playlistService)
        {
            _searchService = searchService;
            _playlistService = playlistService;
        }

        //
        // GET: /Search/

        //public ActionResult Index(string searchTerm)
        //{
        //    IEnumerable<TrackDto> tracks =  _searchService.Search(searchTerm);
        //    var playlists = _playlistService.GetAll(User.Identity.Name);
        //    var viewModel = new SearchResultsViewModel
        //        {
        //            Tracks = tracks,
        //            Playlists = playlists

        //        };
        //    return View(viewModel);
        //}
    }
}