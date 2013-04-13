using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using Common.Entities;
using Services;
using UI.Views;
using WebGarten2;
using WebGarten2.Html;

namespace UI.Controllers
{
    public class PlaylistController
    {
        private readonly PlaylistService _playlistServices = new PlaylistService();

        [HttpMethod("GET", "/playlists")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
                {
                    Content = new PlaylistsView(_playlistServices.GetAll()).AsHtmlContent()
                };
        }

        [HttpMethod("GET", "/playlist/{id}")]
        public HttpResponseMessage Get(int id)
        {
            Playlist playlist = _playlistServices.GetById(id);
            if (playlist == null)
                return new HttpResponseMessage {Content = new ErrorView("404 Playlist não encontrada").AsHtmlContent()};
            IEnumerable<Song> songs = _playlistServices.GetSongs(playlist);
            return new HttpResponseMessage
                {
                    Content = new PlaylistView(playlist, songs).AsHtmlContent()
                };
        }

        [HttpMethod("GET", "/playlist/{id}/edit")]
        public HttpResponseMessage Edit(int id)
        {
            Playlist playlist = _playlistServices.GetById(id);
            IEnumerable<Song> songs = _playlistServices.GetSongs(playlist);
            return new HttpResponseMessage
                {
                    Content = new EditPlaylistView(playlist, songs).AsHtmlContent()
                };
        }

        [HttpMethod("GET", "/playlists/new")]
        public HttpResponseMessage GetNewPlayistForm()
        {
            return new HttpResponseMessage
                {
                    Content = new NewPlaylistView().AsHtmlContent()
                };
        }

        [HttpMethod("POST", "/playlist")]
        public HttpResponseMessage Post(NameValueCollection content)
        {
            var pl = new Playlist
                {
                    Name = content["name"],
                    Description = content["description"]
                };
            _playlistServices.Add(pl);
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUrl.Playlist(pl));
            return resp;
        }

        [HttpMethod("GET", "/playlist/{id}/delete")]
        public HttpResponseMessage Delete(int id)
        {
            var pl = new Playlist {Id = id};
            string response;
            if (_playlistServices.IsEmpty(pl))
            {
                _playlistServices.Delete(pl);
                response = "A playlist foi removida com sucesso!";
            }
            else
                response = "A playlist não foi removida porque contém músicas!";

            return new HttpResponseMessage
                {
                    Content = new ErrorView(response).AsHtmlContent()
                };
        }

        [HttpMethod("POST", "/playlist/{id}/addSong")]
        public HttpResponseMessage AddSong(int id, NameValueCollection content)
        {
            var pl = new Playlist {Id = id};
            var song = new Song {Id = content["SongId"]};
            if (_playlistServices.AddSong(pl, song))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
                resp.Headers.Location = new Uri(ResolveUrl.EditPlaylist(pl));
                return resp;
            }
            return new HttpResponseMessage
                {
                    Content = new ErrorView("Essa musica não existe").AsHtmlContent()
                };
        }

        [HttpMethod("POST", "/playlist/{id}/removeSong")]
        public HttpResponseMessage DeleteSong(int id, NameValueCollection content)
        {
            var pl = new Playlist {Id = id};
            var song = new Song {Id = content["SongId"]};
            _playlistServices.RemoveSong(pl, song);
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUrl.EditPlaylist(pl));
            return resp;
        }

        [HttpMethod("POST", "/playlist/{id}/moveUp")]
        public HttpResponseMessage MoveSongUp(int id, NameValueCollection content)
        {
            var pl = new Playlist {Id = id};
            var song = new Song {Id = content["SongId"]};
            _playlistServices.MoveSongUp(pl, song);
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUrl.EditPlaylist(pl));
            return resp;
        }

        [HttpMethod("POST", "/playlist/{id}/moveDown")]
        public HttpResponseMessage MoveSongDown(int id, NameValueCollection content)
        {
            var pl = new Playlist {Id = id};
            var song = new Song {Id = content["SongId"]};
            _playlistServices.MoveSongDown(pl, song);
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUrl.EditPlaylist(pl));
            return resp;
        }
    }
}