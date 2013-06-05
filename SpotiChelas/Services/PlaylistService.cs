using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Dto;
using Persistence.DAO;
using Persistence.Repositories;

namespace Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ITrackRepository _repo;

        public PlaylistService(ITrackRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<PlaylistDto> GetAll(string userId)
        {
            var db = new Db();
            IQueryable<Playlist> playlists = db.Playlists.Where(playlist => playlist.UserId == userId);
            Mapper.CreateMap<Playlist, PlaylistDto>();
            return Mapper.Map<IEnumerable<Playlist>, List<PlaylistDto>>(playlists);
            //return
            //    playlists.Select(
            //        p => new PlaylistDto {Id = p.Id, UserName = p.UserId, Name = p.Name, Description = p.Description}).ToList();
        }

        //public Playlist GetById(string userId, int id)
        //{
        //    using (var db = new Db())
        //    {

        //        var pl = db.Playlists.Include(x => x.PlaylistTracks)
        //                   .FirstOrDefault(playlist => playlist.Id == id && playlist.UserId == userId);
        //        return pl;

        //        //return
        //        //    db.Playlists.Include(x => x.PlaylistTracks)
        //        //      .FirstOrDefault(playlist => playlist.Id == id && playlist.UserId == userId);
        //    }
        //}

        //public void Add(string userId, Playlist pl)
        //{
        //    using (var db = new Db())
        //    {
        //        pl.UserService = new UserProfile {UserId = userId};
        //        db.Playlists.Add(pl);
        //        db.SaveChanges();
        //    }
        //}

        //public bool Delete(string userId, int playlistId)
        //{
        //    using (var db = new Db())
        //    {
        //        Domain.Persistence.DataTables.Playlist playlist =
        //            db.Playlists.First(playlist1 => playlist1.Id == playlistId && playlist1.UserId == userId);
        //        //Se existirem tracks na playlist 
        //        //não pode ser apagada
        //        if (playlist.PlaylistTracks.Any())
        //            return false;
        //        db.Playlists.Remove(playlist);
        //        db.SaveChanges();
        //        return true;
        //    }
        //}

        //public IEnumerable<SpotifyTrack> GetTracks(Playlist playlist)
        //{
        //    IEnumerable<string> trackIds = playlist.PlaylistTracks.Select(track => track.SpotifyTrackId);
        //    IEnumerable<SpotifyTrack> tracks = _repository.GetTracks(trackIds);
        //    IEnumerable<SpotifyTrack> orderedTracks = from t in tracks
        //                                       join x in playlist.PlaylistTracks on t.Id equals x.SpotifyTrackId
        //                                       orderby x.Order
        //                                       select t;
        //    return orderedTracks;
        //}

        //public void AddTrack(string userId, int playlistId, string songId)
        //{
        //    using (var db = new Db())
        //    {
        //        Playlist playlist =
        //            db.Playlists.Include(playlist1 => playlist1.PlaylistTracks)
        //              .FirstOrDefault(p => p.Id == playlistId && p.UserId == userId);
        //        // && p.UserService==user);
        //        var pltrack = new PlaylistTrack
        //            {
        //                UserId = userId,
        //                Order =
        //                    playlist.PlaylistTracks.Any() ? playlist.PlaylistTracks.Max(track => track.Order) + 1 : 1,
        //                PlaylistId = playlist.Id,
        //                SpotifyTrackId = songId
        //            };
        //        db.PlaylistTracks.Add(pltrack);
        //        db.SaveChanges();
        //    }
        //}

        public void Update(Playlist playlist)
        {
            using (var db = new Db())
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //swap entre a ordem do track de trackid com o acima dele
        public void TrackUp(string userId, int playlistId, string trackId)
        {
            using (var db = new Db())
            {
                //track para mover para cima
                PlaylistTrack playlistTrack =
                    db.PlaylistTracks.First(
                        track => track.PlaylistId == playlistId && track.SpotifyTrackId == trackId
                                 && track.UserId == userId);
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

        public void TrackDown(string userId, int playlistId, string trackId)
        {
            using (var db = new Db())
            {
                //track para mover para cima
                PlaylistTrack playlistTrack =
                    db.PlaylistTracks.First(
                        track => track.PlaylistId == playlistId && track.SpotifyTrackId == trackId
                                 && track.UserId == userId);
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

        public void DeleteTrack(string userId, int playlistId, string trackId)
        {
            using (var db = new Db())
            {
                PlaylistTrack track =
                    db.PlaylistTracks.First(
                        pTrack => pTrack.PlaylistId == playlistId && pTrack.SpotifyTrackId == trackId
                                  && pTrack.UserId == userId);
                db.PlaylistTracks.Remove(track);
                db.SaveChanges();
            }
        }

        public IEnumerable<PlaylistPermission> GetPermissionsGivenBy(string userId)
        {
            using (var db = new Db())
            {
                return
                    db.PlaylistPermissions.Include(permission => permission.Playlist)
                      .Include(permission => permission.GrantedUser)
                      .Where(permission => permission.OwnerId == userId)
                      .ToList();
            }
        }

        public void AddPermission(string ownerId, string grantedUserId, int playlistId, bool contributor)
        {
            using (var db = new Db())
            {
                db.PlaylistPermissions.Add(new PlaylistPermission
                    {
                        OwnerId = ownerId,
                        GrantedUserId = grantedUserId,
                        PlaylistId = playlistId,
                        Contributor = contributor
                    });
                db.SaveChanges();
            }
        }

        public void RemovePermission(string ownerId, string grantedUserId, int playlistId)
        {
            using (var db = new Db())
            {
                PlaylistPermission perm =
                    db.PlaylistPermissions.FirstOrDefault(
                        permission =>
                        permission.OwnerId == ownerId && permission.GrantedUserId == grantedUserId &&
                        permission.PlaylistId == playlistId);
                if (perm == null) return;
                db.PlaylistPermissions.Remove(perm);
                db.SaveChanges();
            }
        }

        public IEnumerable<PlaylistPermission> GetPermmitedPlaylists(string user)
        {
            using (var db = new Db())
            {
                return
                    db.PlaylistPermissions.Include(permission => permission.Owner)
                      .Include(permission => permission.Playlist)
                      .Where(permission => permission.GrantedUserId == user)
                      .ToList();
            }
        }

        public PlaylistPermission GetPermittedPlaylist(string name, int playlistid)
        {
            using (var db = new Db())
            {
                return db.PlaylistPermissions.Include(permission => permission.Playlist)
                         .FirstOrDefault(p => p.GrantedUserId == name && p.PlaylistId == playlistid);
            }
        }
    }
}