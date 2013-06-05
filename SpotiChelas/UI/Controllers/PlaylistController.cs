using System.Collections.Generic;
using System.Web.Mvc;
using Dto;
using Services;

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
                //_playlistService.Add(User.Identity.Name, playlist);
                return RedirectToAction("Index");
            }

            return View(playlist);
        }


        //[HttpGet]
        //public ActionResult Details(int playlistId)
        //{
        //  //  Playlist playlist = _playlistService.GetById(User.Identity.Name, playlistId);
        //    //IEnumerable<SpotifyTrack> tracks = _playlistService.GetTracks(playlist);
        //    var viewModel = new PlaylistViewModel
        //        {
        //        //    Playlist = playlist,
        //          //  Tracks = tracks
        //        };
        //    return View(viewModel);
        //}

        //
        // GET: /Playlists/Edit/5

        //public ActionResult Edit(int playlistId)
        //{
        //   // Playlist playlist = _playlistService.GetById(User.Identity.Name, playlistId);
        //    //IEnumerable<SpotifyTrack> tracks = _playlistService.GetTracks(playlist);
        //    var viewModel = new PlaylistViewModel
        //    {
        //      //  Playlist = playlist,
        //        //Tracks = tracks
        //    };
        //    return View(viewModel);
        //}

        //
        // POST: /Playlists/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Playlist playlist)
        //{
        //    _playlistService.Update(playlist);
        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public ActionResult Delete(int playlistId)
        //{
        //    return null;
        //    // return RedirectToAction(_playlistService.Delete(User.Identity.Name, playlistId) ? "Index" : "Edit");
        //}

        ////invocado no /search
        //[HttpPost]
        //public ActionResult AddTrack(int playlistId, string trackId)
        //{  
        //   //  _playlistService.AddTrack(User.Identity.Name, playlistId,trackId);
        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public ActionResult TrackUp(int playlistId, string trackId)
        //{
        //    _playlistService.TrackUp(User.Identity.Name, playlistId, trackId);
        //    return RedirectToAction("Edit", new{id=playlistId});
        //}

        //[HttpPost]
        //public ActionResult TrackDown(int playlistId, string trackId)
        //{
        //    _playlistService.TrackDown(User.Identity.Name, playlistId, trackId);
        //    return RedirectToAction("Edit", new{id=playlistId});
        //}

        //[HttpPost]
        //public ActionResult DeleteTrack(int playlistId, string trackId)
        //{
        //    _playlistService.DeleteTrack(User.Identity.Name, playlistId, trackId);
        //    return RedirectToAction("Edit", new{id=playlistId});
        //}

        //[HttpGet]
        //public ActionResult ManagePermissions(int playlistId)
        //{
        //    IEnumerable<PlaylistPermission> playlitsPermitted = _playlistService.GetPermissionsGivenBy(User.Identity.Name);
        //    ViewBag.PlaylistId = playlistId;
        //    return View(playlitsPermitted);
        //}

        //[HttpPost]
        //public ActionResult AddPermission(string grantedUser, int playlistId, bool? contributor)
        //{
        //    if(Membership.GetUser(grantedUser) != null)
        //     _playlistService.AddPermission(User.Identity.Name, grantedUser, playlistId, contributor.HasValue);
        //    return RedirectToAction("ManagePermissions");
        //}

        //[HttpPost]
        //public ActionResult RemovePermission(string grantedUser, int playlistId)
        //{
        //    _playlistService.RemovePermission(User.Identity.Name, grantedUser, playlistId);
        //    return RedirectToAction("ManagePermissions");
        //}

        //public ActionResult PermittedPlaylists()
        //{
        //    return null;
        //    //return View(_playlistService.GetPermmitedPlaylists(User.Identity.Name));
        //}

        //public ActionResult PermittedPlayistDetails(int playlistId)
        //{
        //    var pl = _playlistService.GetPermittedPlaylist(UserService.Identity.Name, playlistid);
        //    var tracks = _playlistService.GetTracks(pl.Playlist);
        //    //criar new view model para por os tracks
        //    var vm = new PlaylistPermissionViewModel
        //        {
        //            Permission = pl,
        //            Tracks = tracks
        //        };
        //    return View(vm);
        //}

        //public ActionResult PermittedPlayistEdit(int playlistId)
        //{

        //}
    }
}