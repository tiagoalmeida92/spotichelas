using System.Collections.Generic;
using Domain.Entities;

namespace Services
{
    public interface IPlaylistService
    {
        IEnumerable<Playlist> GetAll();
        Playlist GetById(int id);
        void Add(Playlist pl);
        void Delete(Playlist pl);
        IEnumerable<Track> GetTracks(Playlist playlist);
        void AddTrack(int playlistId, string songId);
        void Update(Playlist playlist);
    }
}