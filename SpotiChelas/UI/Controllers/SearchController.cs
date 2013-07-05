using System.Collections.Generic;
using System.Web.Mvc;
using Dto;
using Services;
using UI.ViewModels;

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

        public ActionResult Index(string q, int? page)
        {
            var actualPage = page ?? 1;
            var tracks =  _searchService.Search(q, actualPage);
            var playlists = _playlistService.GetAll(User.Identity.Name);
            var viewModel = new SearchResultsViewModel
                {
                    Tracks = tracks,
                    Playlists = playlists,
                    SearchTerm = q,
                    Page = actualPage

                };
            return View(viewModel);
        }
    }
}