using System.Collections.Generic;
using AutoMapper;
using Dto;
using Persistence.DAO;
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

        public IEnumerable<TrackDto> Search(string searchTerm)
        {
            IEnumerable<Track> tracks = _repo.Search(searchTerm);
            Mapper.CreateMap<Track, TrackDto>();
            return Mapper.Map<IEnumerable<Track>, List<TrackDto>>(tracks);
        }
    }
}