using System.Collections.Generic;
using Dto;

namespace Services
{
    public interface IPlaylistService
    {
        IEnumerable<PlaylistDto> GetAll(string userId);

        void Add(PlaylistDto pl);
        
        PlaylistDto GetById(string userName, int playlistId);
        
        void AddTrack(string username, int playlistId, string trackId);
        
        void EditTracks(PlaylistDto playlist);
        
        void Delete(string username, int playlistId);

        IEnumerable<SharedPlaylistDto> GetSharedByMe(string name);

        bool AddSharedPlaylist(SharedPlaylistDto sharedPlaylist);
    }
}