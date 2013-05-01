using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Entities;
using Services;
using UI.ViewModels;

namespace UI.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService = new PlaylistService();

        //
        // GET: /Playlist/

        public ViewResult Index()
        {
            var playlists = _playlistService.GetAll();
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
        public ActionResult Create(Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                _playlistService.Add(playlist);
                return RedirectToAction("Index");
            }

            return View(playlist);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            Playlist playlist = _playlistService.GetById(id);


            IEnumerable<Track> tracks = _playlistService.GetTracks(playlist);


            var viewModel = new PlaylistViewModel
                {
                    Playlist = playlist,
                    Tracks = tracks
                };
            return View(viewModel);
        }

        //
        // GET: /Playlists/Edit/5

        public ActionResult Edit(int id)
        {
            Playlist playlist = _playlistService.GetById(id);
            return View(playlist);
        }


        [HttpPost]
        public ActionResult AddTrack(int playlistId, string trackId)
        {
            
             _playlistService.AddTrack(playlistId,trackId);
            return RedirectToAction("Index");
        }

        ////
        //// POST: /Playlists/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Playlist playlist)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Entry(playlist).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(playlist);
        //}

        ////
        //// GET: /Playlists/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    Playlist playlist = _db.Playlists.Find(id);
        //    return View(playlist);
        //}

        ////
        //// POST: /Playlists/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Playlist playlist = _db.Playlists.Find(id);
        //    _db.Playlists.Remove(playlist);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}