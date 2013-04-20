using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;

namespace UI.Controllers
{ 
    public class PlaylistController : Controller
    {
        private readonly PlaylistDb _db = new PlaylistDb();

        //
        // GET: /Playlist/

        public ViewResult Index()
        {
            return View(_db.Playlists.ToList());
        }

        //
        // GET: /Playlist/Details/5

        public ViewResult Details(int id)
        {
            Playlist playlist = _db.Playlists.Find(id);
            return View(playlist);
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
                _db.Playlists.Add(playlist);
                _db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(playlist);
        }
        
        //
        // GET: /Playlist/Edit/5
 
        public ActionResult Edit(int id)
        {
            Playlist playlist = _db.Playlists.Find(id);
            return View(playlist);
        }

        //
        // POST: /Playlist/Edit/5

        [HttpPost]
        public ActionResult Edit(Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(playlist).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(playlist);
        }

        //
        // GET: /Playlist/Delete/5
 
        public ActionResult Delete(int id)
        {
            Playlist playlist = _db.Playlists.Find(id);
            return View(playlist);
        }

        //
        // POST: /Playlist/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Playlist playlist = _db.Playlists.Find(id);
            _db.Playlists.Remove(playlist);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if(_db != null)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}