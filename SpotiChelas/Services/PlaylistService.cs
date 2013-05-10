using System.Collections.Generic;
using System.Data;
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

        public bool Delete(int playlistId)
        {
            using (var db = new PlaylistDb())
            {
                //Se existirem tracks na playlist 
                //não pode ser apagada
                if (db.PlaylistTracks.Any(track => track.PlaylistId == playlistId))
                    return false;
                var playlist = db.Playlists.First(playlist1 => playlist1.Id == playlistId);
                db.Playlists.Remove(playlist);
                db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<Track> GetTracks(Playlist playlist)
        {
            IEnumerable<string> trackIds = playlist.PlaylistTracks.Select(track => track.SpotifyTrackId);
            IEnumerable<Track> tracks = _repository.GetTracks(trackIds);
            IEnumerable<Track> orderedTracks = from t in tracks
                                               join x in playlist.PlaylistTracks on t.Id equals x.SpotifyTrackId
                                               orderby x.Order
                                               select t;
            return orderedTracks;
        }

        public void AddTrack(int playlistId, string songId)
        {
            using (var db = new PlaylistDb())
            {
                Playlist playlist =
                    db.Playlists.Include(playlist1 => playlist1.PlaylistTracks).FirstOrDefault(p => p.Id == playlistId);
                var pltrack = new PlaylistTrack
                    {
                        Order =
                            playlist.PlaylistTracks.Any() ? playlist.PlaylistTracks.Max(track => track.Order) + 1 : 1,
                        PlaylistId = playlist.Id,
                        SpotifyTrackId = songId
                    };
                db.PlaylistTracks.Add(pltrack);
                db.SaveChanges();
            }
        }

        public void Update(Playlist playlist)
        {
            using (var db = new PlaylistDb())
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //swap entre a ordem do track de trackid com o acima dele
        public void TrackUp(int playlistId, string trackId)
        {
            using (var db = new PlaylistDb())
            {
                //track para mover para cima
                PlaylistTrack playlistTrack =
                    db.PlaylistTracks.First(
                        track => track.PlaylistId == playlistId && track.SpotifyTrackId == trackId);
                //track com Order mais baixo
                PlaylistTrack playlistWithLowerPos =
                    db.PlaylistTracks.FirstOrDefault(
                        track => track.PlaylistId == playlistId && track.Order < playlistTrack.Order);

                if (playlistWithLowerPos == null) //Não há nenhum acima
                    return;
                //Troca de ordens
                int tmp = playlistTrack.Order;
                playlistTrack.Order = playlistWithLowerPos.Order;
                playlistWithLowerPos.Order = tmp;

                db.Entry(playlistTrack).State = EntityState.Modified;
                db.Entry(playlistWithLowerPos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void TrackDown(int playlistId, string trackId)
        {
            using (var db = new PlaylistDb())
            {
                //track para mover para cima
                PlaylistTrack playlistTrack =
                    db.PlaylistTracks.First(
                        track => track.PlaylistId == playlistId && track.SpotifyTrackId == trackId);
                //track com Order mais alto
                PlaylistTrack playlistWithHigherPos =
                    db.PlaylistTracks.FirstOrDefault(
                        track => track.PlaylistId == playlistId && track.Order > playlistTrack.Order);

                if (playlistWithHigherPos == null) //Não há nenhum abaixo
                    return;
                //Troca de ordens
                int tmp = playlistTrack.Order;
                playlistTrack.Order = playlistWithHigherPos.Order;
                playlistWithHigherPos.Order = tmp;

                db.Entry(playlistTrack).State = EntityState.Modified;
                db.Entry(playlistWithHigherPos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteTrack(int playlistId, string trackId)
        {
            using (var db = new PlaylistDb())
            {
                PlaylistTrack track =
                    db.PlaylistTracks.First(pTrack => pTrack.PlaylistId == playlistId && pTrack.SpotifyTrackId == trackId);
                db.PlaylistTracks.Remove(track);
                db.SaveChanges();
            }
        }
    }
}