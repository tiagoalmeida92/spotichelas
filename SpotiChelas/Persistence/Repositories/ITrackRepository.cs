using System;
using System.Collections.Generic;
using Persistence.DO;

namespace Persistence.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetTracks(IEnumerable<String> trackIds);

        IEnumerable<Track> Search(string searchTerm, int page);
    }
}