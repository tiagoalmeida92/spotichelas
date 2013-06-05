using System;
using System.Collections.Generic;
using Persistence.DAO;

namespace Persistence.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetTracks(IEnumerable<String> trackIds);

        IEnumerable<Track> Search(string searchTerm);
    }
}