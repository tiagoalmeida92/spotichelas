using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using Dto;
using Services;
using UI.ViewModels;

namespace UI.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        //
        // GET: /Playlist/

        public ViewResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            IEnumerable<PlaylistDto> playlists = _playlistService.GetAll(userName);
            return View(playlists);
        }


        //
        // GET: /Playlist/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Playlist/Create

        [HttpPost]
        public ActionResult Create(PlaylistDto playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.UserId = HttpContext.User.Identity.Name;
                _playlistService.Add(playlist);
                return RedirectToAction("Index");
            }
            return View(playlist);
        }


        [HttpGet]
        public ActionResult Edit(int playlistId)
        {
            var playlistDto = _playlistService.GetById(User.Identity.Name, playlistId);
            return View(playlistDto);
        }

 

        //AJAX
        [HttpPost]
        public ActionResult AddTrack(int playlistId, string trackId)
        {  
            _playlistService.AddTrack(User.Identity.Name, playlistId, trackId);
            return new EmptyResult();
        }

        //AJAX
        [HttpPost]
        public ActionResult EditTracks(PlaylistDto playlist)
        {
            _playlistService.EditTracks(playlist);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Delete(int playlistId)
        {

            _playlistService.Delete(User.Identity.Name, playlistId);
            return RedirectToAction("Index");
        }

        public ActionResult Share()
        {
            var user = User.Identity.Name;
            var sharedPlaylists = _playlistService.GetSharedByMe(user);
            var allPlaylists = _playlistService.GetAll(user);
            return View(new SharePlaylistsViewModel
                {
                    Playlists = allPlaylists,
                    SharedPlaylists = sharedPlaylists
                });
        }

        //AJAX
        [HttpPost]
        public ActionResult SharePlaylist(SharedPlaylistDto sharedPlaylist)
        {
            if (_playlistService.AddSharedPlaylist(sharedPlaylist))
            {
                return PartialView("DisplayTemplates/SharedPlaylistDto", sharedPlaylist);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}