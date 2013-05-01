using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.Entities;
using Domain.Persistence.Repositories;

namespace Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly TrackWebRepository _repository = new TrackWebRepository();


        public IEnumerable<Playlist> GetAll()
        {
            using (var db = new PlaylistDb())
                return db.Playlists.ToList();
        }

        public Playlist GetById(int id)
        {
            using (var db = new PlaylistDb())
            {
                return db.Playlists.Include(x => x.PlaylistTracks).FirstOrDefault(playlist => playlist.Id == id);
            }
        }

        public void Add(Playlist pl)
        {
            using (var db = new PlaylistDb())
            {
                db.Playlists.Add(pl);
                db.SaveChanges();
            }
        }

        public void Delete(Playlist pl)
        {
            using (var db = new PlaylistDb())
            {
                db.Playlists.Remove(pl);
                db.SaveChanges();
            }
        }

        public IEnumerable<Track> GetTracks(Playlist playlist)
        {
            IEnumerable<string> orderedTrackIds = from t in playlist.PlaylistTracks
                                                  orderby t.Order
                                                  select t.SpotifyTrackId;
            return _repository.GetTracks(orderedTrackIds.ToArray());
        }

        public void AddTrack(int playlistId, string songId)
        {
            using (var db = new PlaylistDb())
            {
                var playlist = db.Playlists.Include(playlist1 => playlist1.PlaylistTracks).FirstOrDefault(p => p.Id == playlistId);
                var pltrack = new PlaylistTrack
                    {
                        Order = playlist.PlaylistTracks.Any() ? playlist.PlaylistTracks.Max(track => track.Order) + 1 : 1,
                        PlaylistId = playlist.Id,
                        SpotifyTrackId = songId
                    };
                db.PlaylistTracks.Add(pltrack);
                db.SaveChanges();

            }
        }
    }
}