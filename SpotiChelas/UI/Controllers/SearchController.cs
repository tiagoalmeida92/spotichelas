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

        ISearchService _searchService = new SearchService();
        IPlaylistService _playlistService = new PlaylistService();
        
        //
        // GET: /Search/

        public ActionResult Index(string searchTerm)
        {
            IEnumerable<Track> tracks =  _searchService.Search(searchTerm);
            IEnumerable<Playlist> playlists = _playlistService.GetAll();
            var viewModel = new SearchResultsViewModel
                {
                    Tracks = tracks,
                    Playlists = playlists

                };
            return View(viewModel);
        }
    }
}