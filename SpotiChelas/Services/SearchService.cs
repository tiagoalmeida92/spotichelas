using System.Collections.Generic;
using Domain.Entities;
using Domain.Persistence.Repositories;

namespace Services
{
    public class SearchService 
    {
        private readonly ITrackRepository _repo = new TrackWebRepository();

        public IEnumerable<Track> Search(string searchTerm)
        {
            return _repo.Search(searchTerm);
        }
    }
}