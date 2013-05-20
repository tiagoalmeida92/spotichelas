using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace UI.ViewModels
{
    public class PlaylistPermissionViewModel
    {
        public PlaylistPermission Permission { get; set; }

        public IEnumerable<Track> Tracks { get; set; }
    }
}