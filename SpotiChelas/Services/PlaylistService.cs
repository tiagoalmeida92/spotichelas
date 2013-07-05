using System.Collections;
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
            IQueryable<Playlist> playlists =
                _db.Playlists.Include(p => p.Tracks).Where(playlist => playlist.UserId == userId);
            Mapper.CreateMap<Playlist, PlaylistDto>()
                .ForMember(dto => dto.TotalTracks, e => e.MapFrom(playlist => playlist.Tracks.Count))
                .ForMember(dto => dto.Tracks, e => e.Ignore());
            return Mapper.Map<IEnumerable<Playlist>, IEnumerable<PlaylistDto>>(playlists);
        }

        public void Add(PlaylistDto pl)
        {
            Mapper.CreateMap<PlaylistDto, Playlist>();
            Playlist playlist = Mapper.Map<PlaylistDto, Playlist>(pl);
            _db.Entry(playlist).State = EntityState.Added;
            _db.SaveChanges();
        }

        public PlaylistDto GetById(string user, int id)
        {
            Playlist playlist = _db.Playlists.Find(id);
            var contributor = true;
            if(playlist.UserId != user) // pode ser partilhada
            {
                UserProfile up =
                    _db.UserProfiles.Include(u => u.SharedPlaylistsToMe).FirstOrDefault(u => u.UserId == user);
                if (up == null) return null;
                SharedPlaylist sr = up.SharedPlaylistsToMe.FirstOrDefault(p => p.PlaylistId == id);
                if (sr == null) return null;
                contributor = sr.Contributor;
            } 
            Mapper.CreateMap<Playlist, PlaylistDto>()
                .ForMember(dto => dto.Tracks, e => e.Ignore()).ForMember(dto=> dto.Contributor, e=>e.MapFrom(p=>contributor));
            PlaylistDto playlistDto = Mapper.Map<Playlist, PlaylistDto>(playlist);
            //mashup happens here
            IEnumerable<Track> tracks =
                _repo.GetTracks(playlist.Tracks
                                    .OrderBy(track => track.Position)
                                    .Select(track => track.SpotifyTrackId));
            Mapper.CreateMap<Track, TrackDto>();
            playlistDto.Tracks = Mapper.Map<IEnumerable<Track>, IEnumerable<TrackDto>>(tracks);
            return playlistDto;
        }

        public void AddTrack(string username, int playlistId, string trackId)
        {
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
            Mapper.CreateMap<SharedPlaylist, SharedPlaylistDto>();
            Mapper.CreateMap<Playlist, PlaylistDto>().ForMember(p=> p.Tracks, e=> e.Ignore());
            return Mapper.Map<IEnumerable<SharedPlaylist>, IEnumerable<SharedPlaylistDto>>(playlists);
        }

        public IEnumerable<SharedPlaylistDto> GetSharedToMe(string username)
        {
            var user = _db.UserProfiles.
                Include(u => u.SharedPlaylistsToMe)
                .Include(u => u.SharedPlaylistsToMe.Select(p => p.Playlist))
                .FirstOrDefault(u => u.UserId == username);
            if (user == null) return null;
            ICollection<SharedPlaylist> playlists = user.SharedPlaylistsToMe;
            Mapper.CreateMap<SharedPlaylist, SharedPlaylistDto>();
            Mapper.CreateMap<Playlist, PlaylistDto>().ForMember(p=>p.TotalTracks, e=> e.MapFrom(p=> p.Tracks.Count)).ForMember(p=> p.Tracks, e=> e.Ignore());
            return Mapper.Map<IEnumerable<SharedPlaylist>, IEnumerable<SharedPlaylistDto>>(playlists);
        }

        public bool AddSharedPlaylist(SharedPlaylistDto sharedPlaylistDto)
        {
            if (sharedPlaylistDto.OwnerId == sharedPlaylistDto.UserId ||
                !_db.UserProfiles.Any(u => u.UserId == sharedPlaylistDto.UserId))
                return false;
            Mapper.CreateMap<SharedPlaylistDto, SharedPlaylist>();
            SharedPlaylist sharedPlaylist = Mapper.Map<SharedPlaylistDto, SharedPlaylist>(sharedPlaylistDto);
            UserProfile owner = _db.UserProfiles.Find(sharedPlaylist.OwnerId);
            sharedPlaylistDto.Playlist = new PlaylistDto{Name = _db.Playlists.FirstOrDefault(p=> p.Id == sharedPlaylistDto.PlaylistId).Name};
            if (owner.SharedPlaylistsByMe.Any(
                e => e.PlaylistId == sharedPlaylist.PlaylistId && e.UserId == sharedPlaylist.UserId))
            {
                _db.Entry(sharedPlaylist).State = EntityState.Modified;
                _db.SaveChanges();
                return false;
            }
            else
            {
                _db.Entry(sharedPlaylist).State = EntityState.Added;
                _db.SaveChanges();
                return true;
            }
        }

        public void RemoveSharedPlaylist(SharedPlaylistDto sharedPlaylistDto)
        {
            Mapper.CreateMap<SharedPlaylistDto, SharedPlaylist>();
            SharedPlaylist sharedPlaylist = Mapper.Map<SharedPlaylistDto, SharedPlaylist>(sharedPlaylistDto);
            _db.Entry(sharedPlaylist).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        #endregion
    }
}