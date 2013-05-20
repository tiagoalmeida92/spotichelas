using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Entities;
using Services;
using UI.ViewModels;

namespace UI.Controllers
{
    public class SearchController : Controller
    {
        readonly SearchService _searchService = new SearchService();
        readonly PlaylistService _playlistService = new PlaylistService();
        
        //
        // GET: /Search/

        public ActionResult Index(string searchTerm)
        {
            IEnumerable<Track> tracks =  _searchService.Search(searchTerm);
            var playlists = _playlistService.GetAll(User.Identity.Name);
            var viewModel = new SearchResultsViewModel
                {
                    Tracks = tracks,
                    Playlists = playlists

                };
            return View(viewModel);
        }
    }
}