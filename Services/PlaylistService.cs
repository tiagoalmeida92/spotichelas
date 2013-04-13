using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using DAL.Concrete;

namespace Services
{
    public class PlaylistService
    {
        public IEnumerable<Playlist> GetAll()
        {
            using (var repo = new DbPlaylistRepository())
                return repo.GetAll();
        }

        public Playlist GetById(int id)
        {
            using (var repo = new DbPlaylistRepository())
                return repo.GetById(id);
        }


        public void Add(Playlist pl)
        {
            using (var repo = new DbPlaylistRepository())
                repo.Add(pl);
        }

        public IEnumerable<Song> GetSongs(Playlist pl)
        {
            using (var playlistRepo = new DbPlaylistRepository())
            using (var songRepo = new SpotifyMusicWebRepository())
            {
                IEnumerable<string> tracks = playlistRepo.GetSongIds(pl);

                foreach (string songId in tracks)
                    yield return songRepo.GetById(songId);
            }
        }


        public bool IsEmpty(Playlist pl)
        {
            using (var playlistRepo = new DbPlaylistRepository())
                return !playlistRepo.GetSongIds(pl).Any();
        }

        public void Delete(Playlist pl)
        {
            using (var playlistRepo = new DbPlaylistRepository())
                playlistRepo.Delete(pl);
        }

        public bool AddSong(Playlist pl, Song song)
        {
            using (var playlistRepo = new DbPlaylistRepository())
            using (var songRepo = new SpotifyMusicWebRepository())
            {
                if (songRepo.GetById(song.Id) == null)
                    return false;
                playlistRepo.AddSong(pl, song);
                return true;
            }
        }

        public void RemoveSong(Playlist pl, Song song)
        {
            using (var playlistRepo = new DbPlaylistRepository())
                playlistRepo.AddSong(pl, song);
        }

        public void MoveSongUp(Playlist pl, Song song)
        {
            using (var playlistRepo = new DbPlaylistRepository())
                playlistRepo.MoveSongUp(pl, song);
        }

        public void MoveSongDown(Playlist pl, Song song)
        {
            using (var playlistRepo = new DbPlaylistRepository())
                playlistRepo.MoveSongDown(pl, song);
        }
    }
}