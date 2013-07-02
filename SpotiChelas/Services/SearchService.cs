using System.Collections.Generic;
using AutoMapper;
using Dto;
using Persistence.DO;
using Persistence.Repositories;

namespace Services
{
    public class SearchService : ISearchService
    {
        private readonly ITrackRepository _repo;


        public SearchService(ITrackRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<TrackDto> Search(string searchTerm, int page)
        {
            var tracks = _repo.Search(searchTerm, page);
            Mapper.CreateMap<Track, TrackDto>();
            return Mapper.Map<IEnumerable<Track>, IEnumerable<TrackDto>>(tracks);
        }
    }
}