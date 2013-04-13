using System.Collections.Generic;
using Common.Entities;
using DAL.Concrete;

namespace Services
{
    public class SearchService
    {
        public IEnumerable<Song> SearchTracks(string s)
        {
            using (var repo = new SpotifyMusicWebRepository())
                return repo.Search(s);
        }
    }
}