using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dto;

namespace UI.ViewModels
{
    public class SharePlaylistsViewModel
    {
        public IEnumerable<PlaylistDto> Playlists { get; set; }
        public IEnumerable<SharedPlaylistDto> SharedPlaylists { get; set; }

    }
}