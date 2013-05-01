using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Persistence.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetTracks(params String[] trackIds);

        IEnumerable<Track> Search(string searchTerm);
    }
}