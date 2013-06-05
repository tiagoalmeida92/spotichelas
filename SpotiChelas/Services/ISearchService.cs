using System.Collections.Generic;
using Dto;

namespace Services
{
    public interface ISearchService
    {
        IEnumerable<TrackDto> Search(string searchTerm);
    }
}