using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public IEnumerable<PlaylistDto> GetAll(string userId)
        {
            var user =
                _db.UserProfiles.Include(u => u.Playlists)
                   .Include(u => u.SharedPlaylistsToMe.Select(s => s.Playlist))
                   .First(u => u.UserId == userId);
            var playlists = user.Playlists.Concat(user.SharedPlaylistsToMe.Select(s=>s.Playlist));
            Mapper.CreateMap<Playlist, PlaylistDto>()
                  .ForMember(dto => dto.TotalTracks, e => e.MapFrom(playlist => playlist.Tracks.Count))
                  .ForMember(dto => dto.Tracks, e => e.Ignore());
            return Mapper.Map<IEnumerable<Playlist>, IEnumerable<PlaylistDto>>(playlists);
        }

        public void Add(PlaylistDto pl)
        {
            Mapper.CreateMap<PlaylistDto, Playlist>();
            var playlist = Mapper.Map<PlaylistDto, Playlist>(pl);
            _db.Entry(playlist).State = EntityState.Added;
            _db.SaveChanges();
        }

        public PlaylistDto GetById(string user, int id)
        {
            //mashup happens here

            var playlist = _db.Playlists.Find(id);
            Mapper.CreateMap<Playlist, PlaylistDto>()
                  .ForMember(dto => dto.Tracks, e => e.Ignore());
            var playlistDto = Mapper.Map<Playlist, PlaylistDto>(playlist);
            var tracks =
                _repo.GetTracks(playlist.Tracks
                                        .OrderBy(track => track.Position)
                                        .Select(track => track.SpotifyTrackId));
            Mapper.CreateMap<Track, TrackDto>();
            playlistDto.Tracks = Mapper.Map<IEnumerable<Track>, IEnumerable<TrackDto>>(tracks);
            return playlistDto;
        }

        public void AddTrack(string username, int playlistId, string trackId)
        {
            var pl = _db.Playlists.Include(p => p.Tracks).SingleOrDefault(p => p.Id == playlistId);
            var pos = 0;
            if (pl.Tracks.Any())
                pos = pl.Tracks.Max(t => t.Position) + 1;
            pl.Tracks.Add(new PlaylistTrack
                {
                    SpotifyTrackId = trackId,
                    Position = pos
                });
            _db.SaveChanges();
        }

        public void EditTracks(PlaylistDto playlist)
        {
            var pl = _db.Playlists.Include(p => p.Tracks).First(p => p.Id == playlist.Id);
            var sortedIds = playlist.Tracks == null
                                ? new List<string>()
                                : playlist.Tracks.Select(dto => dto.Id).ToList();
            var i = 0;
            foreach (var track in pl.Tracks.ToList())
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

        public IEnumerable<SharedPlaylistDto> GetSharedByMe(string name)
        {
            var playlists = _db.UserProfiles.
                                Include(u => u.SharedPlaylistsByMe)
                               .Include(u => u.SharedPlaylistsByMe.Select(e => e.Playlist))
                               .First(u => u.UserId == name).SharedPlaylistsByMe;
            Mapper.CreateMap<SharedPlaylist, SharedPlaylistDto>()
                  .ForMember(e => e.PlaylistName, e => e.MapFrom(p => p.Playlist.Name));
            return Mapper.Map<IEnumerable<SharedPlaylist>, IEnumerable<SharedPlaylistDto>>(playlists);
        }

        public bool AddSharedPlaylist(SharedPlaylistDto sharedPlaylistDto)
        {
            Mapper.CreateMap<SharedPlaylistDto, SharedPlaylist>();
            var sharedPlaylist = Mapper.Map<SharedPlaylistDto, SharedPlaylist>(sharedPlaylistDto);
            var owner = _db.UserProfiles.Find(sharedPlaylist.OwnerId);
            var exists =
                owner.SharedPlaylistsByMe.FirstOrDefault(
                    e => e.PlaylistId == sharedPlaylist.PlaylistId && e.UserId == sharedPlaylist.UserId);
            if (exists != null)
            {
                _db.Entry(exists).CurrentValues.SetValues(sharedPlaylist);
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
    }
}