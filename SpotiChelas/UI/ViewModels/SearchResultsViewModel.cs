using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace UI.ViewModels
{
    public class SearchResultsViewModel
    {
        public IEnumerable<Playlist> Playlists { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }
}