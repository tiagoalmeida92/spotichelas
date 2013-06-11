using System.Collections.Generic;

namespace Persistence.DO
{
    public class Album
    {
        public string released { get; set; }
        public string href { get; set; }
        public string name { get; set; }
    }

    public class Artist
    {
        public string href { get; set; }
        public string name { get; set; }
    }

    public class SpotifyTrack
    {
        public Album album { get; set; }
        public double length { get; set; }
        public string href { get; set; }
        public List<Artist> artists { get; set; }
        public string name { get; set; }
    }

    public class RootObject
    {
        public List<SpotifyTrack> tracks { get; set; }
        public SpotifyTrack track { get; set; }
    }
}