using System.Collections.Generic;

namespace Domain.Persistence.Helpers.Lookup
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

    public class Track
    {
        public Album album { get; set; }
        public double length { get; set; }
        public string href { get; set; }
        public List<Artist> artists { get; set; }
        public string name { get; set; }
    }

    public class RootObject
    {
        public Track track { get; set; }
    }
}