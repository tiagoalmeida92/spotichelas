using System.Collections.Generic;
using Domain.Entities;

namespace Services
{
    public interface ISearchService
    {
        IEnumerable<Track> Search(string searchTerm);
    }
}