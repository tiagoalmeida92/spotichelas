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
            var playlists = _db.Playlists.Where(playlist => playlist.UserId == userId);
            Mapper.CreateMap<Playlist, PlaylistDto>()
                  .ForMember(dto => dto.TotalTracks,
                             e => e.MapFrom(playlist => playlist.Tracks.Count()))
                  .ForMember(dto => dto.Tracks, e => e.Ignore());
            return Mapper.Map<IEnumerable<Playlist>, IEnumerable<PlaylistDto>>(playlists);
        }

        public void Add(PlaylistDto pl)
        {
            Mapper.CreateMap<PlaylistDto, Playlist>();
            var playlist = Mapper.Map<PlaylistDto, Playlist>(pl);
            _db.Playlists.Add(playlist);
            _db.SaveChanges();
        }

        public PlaylistDto GetById(string user, int id)
        {
            //mashup happens here

            var playlist = _db.Playlists.Include(x => x.Tracks).First(p => p.Id == id && p.UserId == user);
            Mapper.CreateMap<Playlist, PlaylistDto>().ForMember(dto => dto.Tracks, e => e.Ignore());
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
            var pl = _db.Playlists.First(p => p.Id == playlistId && p.UserId == username);
            pl.Tracks.Add(new PlaylistTrack
                {
                    SpotifyTrackId = trackId,
                    Position = pl.Tracks.Max(track => track.Position) + 1
                });
            _db.SaveChanges();
        }

        public void DeleteRows(int id, IEnumerable<string> sortedIds)
        {
            //http://stackoverflow.com/questions/337704/parameterizing-an-sql-in-clause/337817#337817
            // DELETE FROM PlaylistTracks WHERE Id NOT IN (
            //select * from playlisttracks where playlistId = 2 
            //AND SpotifyTrackId not in ('2ZDpTOEN1aWhSZACq5OQDt','0mWiuXuLAJ3Brin3Or2x6v','13F75FZlHVN6zOjpBbfRmP','1sn6iOK93jnp0Hn5BnNOXy','2kN05N1AQQplsgFweFAqYb,4sWk6tbgVPkA8OAk0utIz3')
            var transform = sortedIds.Select(s => "'" + s + "'");
            string inClause = string.Join(",", transform);
            string sql =
                string.Format("DELETE FROM PlaylistTracks WHERE playlistId = {0} AND SpotifyTrackId NOT IN ({1})", id,
                              inClause);
            _db.Database.ExecuteSqlCommand(sql);
        }

        public void EditTracks(PlaylistDto playlist)
        {
            var pl = _db.Playlists.Include(playlist1 => playlist1.Tracks)
                        .First(p => p.Id == playlist.Id && p.UserId == playlist.UserId);
            var sortedIds = playlist.Tracks.Select(dto => dto.Id);
            DeleteRows(playlist.Id, sortedIds);
            var i = 0;
            foreach (var id in sortedIds)
            {
                pl.Tracks.FirstOrDefault(track => track.SpotifyTrackId == id).Position = i;
                ++i;
            }
            _db.SaveChanges();
        }
    }
}