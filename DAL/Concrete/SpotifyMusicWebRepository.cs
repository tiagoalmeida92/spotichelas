using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Entities;
using DAL.Interfaces;
using DAL.Utils;
using Newtonsoft.Json.Linq;

namespace DAL.Concrete
{
    public class SpotifyMusicWebRepository : IMusicRepository, IDisposable
    {
        private const string MediaType = "json";
        private readonly HttpClient _httpClient = new HttpClient();

        public void Dispose()
        {
            _httpClient.Dispose();
        }


        public Song GetById(string songId)
        {
            var song = new Song {Id = songId};
            string url = SpotifyAPIHelper.GetLookupUrl(MediaType, SpotifyAPIResource.track, songId);
            string json = Get(url).Result;
            JToken track = JObject.Parse(json)["track"];
            song.Name = track["name"].ToString();
            double length = Convert.ToDouble(track["length"].ToString());
            song.Duration = TimeSpan.FromSeconds(length);
            JEnumerable<JToken> artists = JObject.Parse(json)["track"]["artists"].Children();
            song.Artist = artists.Count() > 1 ? "Vários" : artists.FirstOrDefault()["name"].ToString();
            return song;
        }


        public IEnumerable<Song> Search(string s)
        {
            string url = SpotifyAPIHelper.GetSearchUrl(MediaType, SpotifyAPIResource.track, s);
            string trackResultsJson = Get(url).Result;
            var results = new List<Song>();
            Deserialize(trackResultsJson, results);
            return results;
        }


        private static void Deserialize(string trackResultsJson, List<Song> songResults)
        {
            JToken tracks = JObject.Parse(trackResultsJson)["tracks"];
            songResults.AddRange(from track in tracks
                                 let trackUrl = track["href"].ToString()
                                 select new Song
                                     {
                                         Name = track["name"].ToString(),
                                         Duration = TimeSpan.FromSeconds(Convert.ToDouble(track["length"].ToString())),
                                         Id =
                                             trackUrl.Substring(trackUrl.LastIndexOf(":", StringComparison.Ordinal) + 1),
                                         Artist = (track["artists"].Children().Count() > 1
                                                       ? "Vários"
                                                       : track["artists"].Children().FirstOrDefault()["name"].ToString())
                                     });
        }


        private async Task<string> Get(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}