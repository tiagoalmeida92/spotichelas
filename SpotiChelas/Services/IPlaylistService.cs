using System.Collections.Generic;
using Domain.Entities;

namespace Services
{
    public interface IPlaylistService
    {
        IEnumerable<Playlist> GetAll();
        Playlist GetById(int id);
        void Add(Playlist pl);
        bool Delete(int playlistId);
        IEnumerable<Track> GetTracks(Playlist playlist);
        void AddTrack(int playlistId, string songId);
        void Update(Playlist playlist);
        void TrackUp(int playlistId, string trackId);
        void TrackDown(int playlistId, string trackId);
        void DeleteTrack(int playlistId, string trackId);
    }
}