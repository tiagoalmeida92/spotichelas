using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dto;

namespace UI.ViewModels
{
    public class GetAllViewModel
    {
        public IEnumerable<PlaylistDto> OwnPlaylists { get; set; }

        public IEnumerable<SharedPlaylistDto> SharedPlaylists { get; set; }
    }
}