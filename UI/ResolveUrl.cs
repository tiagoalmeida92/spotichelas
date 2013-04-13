using System;
using Common.Entities;

namespace UI
{
    public static class ResolveUrl
    {
        public static string Playlists()
        {
            return "http://localhost:8080/playlists";
        }

        public static string Playlist()
        {
            return "http://localhost:8080/playlist";
        }

        public static string Playlist(Playlist pl)
        {
            return String.Format("http://localhost:8080/playlist/{0}", pl.Id);
        }

        public static string NewPlaylist()
        {
            return "http://localhost:8080/playlists/new";
        }

        public static string EditPlaylist(Playlist playlist)
        {
            return String.Format("http://localhost:8080/playlist/{0}/edit", playlist.Id);
        }

        public static string RemovePlaylist(Playlist pl)
        {
            return String.Format("http://localhost:8080/playlist/{0}/delete", pl.Id);
        }

        public static string Home()
        {
            return "http://localhost:8080/";
        }

        public static string AddSong(Playlist pl)
        {
            return String.Format("http://localhost:8080/playlist/{0}/addSong", pl.Id);
        }

        public static string DeleteSong(Playlist pl, Song song)
        {
            return String.Format("http://localhost:8080/playlist/{0}/removeSong", pl.Id);
        }

        public static string MoveUp(Playlist pl)
        {
            return String.Format("http://localhost:8080/playlist/{0}/moveUp", pl.Id);
        }

        public static string MoveDown(Playlist pl)
        {
            return String.Format("http://localhost:8080/playlist/{0}/moveDown", pl.Id);
        }

        public static string Search()
        {
            return "http://localhost:8080/search";
        }
    }
}