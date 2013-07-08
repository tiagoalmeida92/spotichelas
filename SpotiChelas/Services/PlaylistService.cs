using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Dto;
using Persistence.DO;
using Persistence.Repositories;

namespace Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly Db _db;
        private readonly ITrackRepository _repo;

        public PlaylistService(ITrackRepository repo, Db db)
        {
            _db = db;
            _repo = repo;
        }

        #region IPlaylistService Members

        public IEnumerable<PlaylistDto> GetAll(string userId)
        {
            var playlists =
                _db.Playlists.Include(p => p.Tracks).Where(playlist => playlist.UserId == userId);
            return Mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        public bool Add(PlaylistDto pl)
        {
            //check name unique
            if(_db.Playlists.Any(p=> p.Name == pl.Name && p.UserId == pl.UserId)) return false;
            var playlist = Mapper.Map<Playlist>(pl);
            _db.Entry(playlist).State = EntityState.Added;
            _db.SaveChanges();
            return true;
        }

        public PlaylistDto GetById(string user, int id)
        {
            Playlist playlist = _db.Playlists.Find(id);
            bool contributor = true;
            if(playlist.UserId != user) // pode ser partilhada
            {
                UserProfile up = _db.UserProfiles.Include(u => u.SharedPlaylistsToMe).FirstOrDefault(u => u.UserId == user);
                if (up == null) return null;
                SharedPlaylist sr = up.SharedPlaylistsToMe.FirstOrDefault(p => p.PlaylistId == id);
                if (sr == null) return null;
                contributor = sr.Contributor;
            } 
            var playlistDto = Mapper.Map<PlaylistDto>(playlist);
            playlistDto.Contributor = contributor;
            //mashup happens here
            IEnumerable<Track> tracks =
                _repo.GetTracks(playlist.Tracks
                                    .OrderBy(track => track.Position)
                                    .Select(track => track.SpotifyTrackId));
            Mapper.CreateMap<Track, TrackDto>();
            playlistDto.Tracks = Mapper.Map<IEnumerable<TrackDto>>(tracks);
            return playlistDto;
        }

        public void AddTrack(string username, int playlistId, string trackId)
        {
            if(trackId == null) return;
            Playlist pl = _db.Playlists.Include(p => p.Tracks).SingleOrDefault(p => p.Id == playlistId);
            int pos = 0;
            if (pl.Tracks.Any())
            {
                pos = pl.Tracks.Max(t => t.Position) + 1;
            }
            pl.Tracks.Add(new PlaylistTrack
                              {
                                  SpotifyTrackId = trackId,
                                  Position = pos
                              });
            _db.SaveChanges();
        }

        public void EditTracks(PlaylistDto playlist)
        {
            Playlist pl = _db.Playlists.Include(p => p.Tracks).First(p => p.Id == playlist.Id);
            List<string> sortedIds = playlist.Tracks == null
                                         ? new List<string>()
                                         : playlist.Tracks.Select(dto => dto.Id).ToList();
            int i = 0;
            foreach (PlaylistTrack track in pl.Tracks.ToList())
            {
                if (i < sortedIds.Count)
                {
                    track.SpotifyTrackId = sortedIds[i];
                    _db.Entry(track).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(track).State = EntityState.Deleted;
                }
                ++i;
            }
            _db.SaveChanges();
        }

        public void Delete(string userId, int playlistId)
        {
            var pl = new Playlist {UserId = userId, Id = playlistId};
            _db.Entry(pl).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public IEnumerable<SharedPlaylistDto> GetSharedByMe(string username)
        {
            ICollection<SharedPlaylist> playlists = _db.UserProfiles.
                Include(u => u.SharedPlaylistsByMe)
                .Include(u => u.SharedPlaylistsByMe.Select(e => e.Playlist))
                .First(u => u.UserId == username).SharedPlaylistsByMe;
            return Mapper.Map<IEnumerable<SharedPlaylistDto>>(playlists);
        }

        public IEnumerable<SharedPlaylistDto> GetSharedToMe(string username)
        {
            var user = _db.UserProfiles.
                Include(u => u.SharedPlaylistsToMe)
                .Include(u => u.SharedPlaylistsToMe.Select(p => p.Playlist))
                .FirstOrDefault(u => u.UserId == username);
            if (user == null) return null;
            return Mapper.Map<IEnumerable<SharedPlaylistDto>>(user.SharedPlaylistsToMe);
        }

        public bool AddOrUpdateSharedPlaylist(SharedPlaylistDto dto)
        {
            if (dto.OwnerId == dto.UserId) return false;
            UserProfile user = _db.UserProfiles.Include(u=> u.SharedPlaylistsByMe)
                .Include(u=> u.SharedPlaylistsByMe.Select(s=> s.Playlist))
                .FirstOrDefault(u => u.UserId == dto.OwnerId);
            if(user == null) return false;
            var newSharedPlaylist = Mapper.Map<SharedPlaylist>(dto);
            SharedPlaylist currentShared = user.SharedPlaylistsByMe
                .FirstOrDefault(s=> s.PlaylistId == dto.PlaylistId && s.UserId == dto.UserId);
            if (currentShared != null)
            {
                _db.Entry(currentShared).CurrentValues.SetValues(newSharedPlaylist);
                _db.SaveChanges();
                return false;
            }
            _db.Entry(newSharedPlaylist).State = EntityState.Added;
            _db.SaveChanges();
            dto.Playlist = new PlaylistDto{Name = _db.Playlists.Find(dto.PlaylistId).Name};
            return true;
        }

        public void RemoveSharedPlaylist(SharedPlaylistDto sharedPlaylistDto)
        {
            SharedPlaylist sharedPlaylist = Mapper.Map<SharedPlaylistDto, SharedPlaylist>(sharedPlaylistDto);
            _db.Entry(sharedPlaylist).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        #endregion
    }
}