using System.Collections.Generic;
using Dto;

namespace Services
{
    public interface IPlaylistService
    {
        IEnumerable<PlaylistDto> GetAll(string userId);
    }
}