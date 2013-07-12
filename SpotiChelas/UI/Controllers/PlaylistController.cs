using System.Linq;
using System.Web.Mvc;
using Dto;
using Services;
using UI.ViewModels;

namespace UI.Controllers
{

    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        //
        // GET: /Playlist/
        [Authorize]
        public ViewResult Index()
        {
            var playlists = _playlistService.GetAll(User.Identity.Name);
            var shared = _playlistService.GetSharedToMe(User.Identity.Name);
            return View(new GetAllViewModel
                            {
                                OwnPlaylists = playlists,
                                SharedPlaylists = shared
                            });
        }


        //
        // GET: /Playlist/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Playlist/Create

        //AJAX
        [HttpPost]
        [Authorize]
        public ActionResult Create(PlaylistDto playlist)
        {

            playlist.UserId = User.Identity.Name;
            var success = _playlistService.Add(playlist);

            return new JsonResult
                       {
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                           Data = new {ok = success}
                       };
        }


        [HttpGet]
        [Authorize]
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
        [Authorize]
        public ActionResult EditTracks(PlaylistDto playlist)
        {
            _playlistService.EditTracks(playlist);
            return new EmptyResult();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int playlistId)
        {

            _playlistService.Delete(User.Identity.Name, playlistId);
            return RedirectToAction("Index");
        }

        [Authorize]
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
        [Authorize]
        public ActionResult AddSharePlaylist(SharedPlaylistDto sharedPlaylist)
        {
            if (_playlistService.AddOrUpdateSharedPlaylist(sharedPlaylist))
            {
                return PartialView("DisplayTemplates/SharedPlaylistDto", sharedPlaylist);
            }
            return new EmptyResult();
        }

        //AJAX
        [HttpPost]
        [Authorize]
        public ActionResult RemoveSharedPlaylist(SharedPlaylistDto sharedPlaylist)
        {
            _playlistService.RemoveSharedPlaylist(sharedPlaylist);
            return new EmptyResult();
        }

        [ChildActionOnly]
        [Authorize]
        public ActionResult GetPlaylistsForAddButton()
        {
            var playlists = _playlistService.GetAll(User.Identity.Name);
            var shared = _playlistService.GetSharedToMe(User.Identity.Name);
            if(shared != null) shared = shared.Where(s => s.Contributor);
            return PartialView(new GetAllViewModel
                            {
                                OwnPlaylists = playlists,
                                SharedPlaylists = shared
                            });
        }

        //AJAX     
        public ActionResult Play(string id)
        {
            return PartialView((object) id);
        }

    }
}