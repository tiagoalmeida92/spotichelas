using System.Collections.Generic;
using Domain.Entities;

namespace UI.ViewModels
{
    public class PlaylistViewModel
    {
        public Playlist Playlist { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }
}