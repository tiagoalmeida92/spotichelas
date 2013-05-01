using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Persistence.Helpers;
using Domain.Persistence.Helpers.Lookup;
using Newtonsoft.Json;
using Track = Domain.Entities.Track;

namespace Domain.Persistence.Repositories
{
    public class TrackWebRepository : ITrackRepository
    {
        private const string JsonMediaType = "json";

        public IEnumerable<Track> GetTracks(params string[] trackIds)
        {
            var tracks = new List<Track>();
            Parallel.ForEach(trackIds, trackId =>
                {
                    string url = SpotifyAPIHelper.GetLookupUrl(JsonMediaType, SpotifyAPIResource.track, trackId);
                    WebRequest req = WebRequest.Create(url);
                    var webResponse = req.GetResponse() as HttpWebResponse;
                    var reader = new StreamReader(webResponse.GetResponseStream());
                    string jsonResult = reader.ReadToEnd();
                    reader.Close();
                    var obj = JsonConvert.DeserializeObject<RootObject>(jsonResult);
                    var track = new Track
                        {
                            Artist = obj.track.artists.Count > 2 ? "Varios" : obj.track.artists[0].name,
                            Id = obj.track.href.Substring(obj.track.href.LastIndexOf(":") + 1),
                            Duration = TimeSpan.FromSeconds(Convert.ToDouble(obj.track.length)),
                            Name = obj.track.name
                        };
                    tracks.Add(track);
                }
                );
            return tracks;
        }

        public IEnumerable<Track> Search(string searchTerm)
        {
            string url = SpotifyAPIHelper.GetSearchUrl(JsonMediaType, SpotifyAPIResource.track, searchTerm);
            WebRequest req = WebRequest.Create(url);
            var webResponse = req.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(webResponse.GetResponseStream());
            string jsonResult = reader.ReadToEnd();
            reader.Close();
            var obj = JsonConvert.DeserializeObject<Domain.Persistence.Helpers.Search.RootObject>(jsonResult);

            return (from t in obj.tracks
                    select new Track
                    {
                        Artist = t.artists.Count > 2 ? "Varios" : t.artists[0].name,
                        Id = t.href.Substring(t.href.LastIndexOf(":") + 1),
                        Duration = TimeSpan.FromSeconds(Convert.ToDouble(t.length)),
                        Name = t.name
                    });
        }

    }
}