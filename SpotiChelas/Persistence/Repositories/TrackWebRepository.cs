using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Persistence.DAO;
using Persistence.Helpers;

namespace Persistence.Repositories
{
    public class TrackWebRepository : ITrackRepository
    {
        private const string JsonMediaType = "json";

        public IEnumerable<Track> GetTracks(IEnumerable<String> trackIds)
        {
            var tracks = new List<Track>();
            Parallel.ForEach(trackIds, trackId =>
                {
                    string url = SpotifyAPIHelper.GetLookupUrl(JsonMediaType, SpotifyAPIResource.track, trackId);
                    RootObject result = SpotifyRequestAndDeserialize(url);
                    tracks.Add(GenerateTrack(result.track));
                }
                );
            return tracks;
        }


        public IEnumerable<Track> Search(string searchTerm)
        {
            string url = SpotifyAPIHelper.GetSearchUrl(JsonMediaType, SpotifyAPIResource.track, searchTerm);
            RootObject result = SpotifyRequestAndDeserialize(url);
            return (from t in result.tracks
                    select GenerateTrack(t));
        }

        private static RootObject SpotifyRequestAndDeserialize(string url)
        {
            WebRequest req = WebRequest.Create(url);
            var webResponse = req.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(webResponse.GetResponseStream());
            string jsonResult = reader.ReadToEnd();
            reader.Close();
            return JsonConvert.DeserializeObject<RootObject>(jsonResult);
        }

        private static Track GenerateTrack(SpotifyTrack track)
        {
            return new Track
                {
                    Artist = track.artists.Count > 2 ? "Varios" : track.artists[0].name,
                    Id = track.href.Substring(track.href.LastIndexOf(":", StringComparison.Ordinal) + 1),
                    Duration = TimeSpan.FromSeconds(Convert.ToDouble(track.length)),
                    Name = track.name
                };
        }
    }
}