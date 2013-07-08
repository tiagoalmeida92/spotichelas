using System.Collections.Generic;
using System.Linq;
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
            var tracks = _searchService.Search(q, actualPage);
            var viewModel = new SearchResultsViewModel
                                {
                                    Tracks = tracks,
                                    SearchTerm = q,
                                    Page = actualPage
                                };
            return View(viewModel);
        }
    }
}