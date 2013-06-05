using System;

namespace Persistence.Helpers
{
    internal static class SpotifyAPIHelper
    {
        private const string Lookup = "lookup";
        private const string Search = "search";


        private const string BaseUrl = "http://ws.spotify.com/";
        private const int APIVersion = 1;


        public static string GetLookupUrl(string mediaType, SpotifyAPIResource resource, string resourceId)
        {
            //http://ws.spotify.com/lookup/1/.json?uri=spotify:track:6NmXV4o6bmp704aPGyTVVG
            return String.Format("{0}{1}/{2}/.{3}?uri=spotify:{4}:{5}", BaseUrl, Lookup, APIVersion, mediaType, resource,
                                 resourceId);
        }

        public static string GetSearchUrl(string mediaType, SpotifyAPIResource resource, string q)
        {
            //http://ws.spotify.com/search/1/album.json?q=foo
            return String.Format("{0}{1}/{2}/{3}.{4}?q={5}", BaseUrl, Search, APIVersion, resource, mediaType, q);
        }
    }


    internal enum SpotifyAPIResource
    {
        track,
        album,
        artist
    }
}