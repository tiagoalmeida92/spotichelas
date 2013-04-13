using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.Entities;
using DAL.Interfaces;
using DAL.Utils;

namespace DAL.Concrete
{
    //IDisposable? connection as field
    public class DbPlaylistRepository : IPlaylistRepository, IDisposable
    {
        private readonly SqlConnection _con;

        public DbPlaylistRepository()
        {
            _con = new SqlConnection(SqlHelper.ConnectionString);
            _con.Open();
        }

        #region SQL SP+Column Names

        //SP names
        private const string UpdateSp = "spPlaylistUpdate";
        private const string InsertSp = "spPlaylistInsert";
        private const string DeleteSp = "spPlaylistDelete";
        private const string SelectAllSp = "spPlaylistSelectAll";
        private const string SelectByIdSp = "spPlaylistSelectById";
        private const string SelectTracksSp = "spPlaylistSelectTracks";
        private const string AddTrackSp = "spPlaylistAddTrack";
        private const string RemoveTrackSp = "spPlaylistRemoveTrack";
        private const string MoveSongUpSp = "spPlaylistTrackUp";
        private const string MoveSongDownSp = "spPlaylistTrackDown";


        //Column names
        //Table Playlist
        private const string IdColumn = "Id";
        private const string NameColumn = "Name";
        private const string DescriptionColumn = "Description";
        //Table Playlist Tracks
        private const string IdColumnPlaylistTracks = "PlaylistId";
        private const string SpotifyIdColumnPlaylistTracks = "SpotifyTrackId";

        #endregion

        public void Dispose()
        {
            _con.Dispose();
        }

        public IEnumerable<Playlist> GetAll()
        {
            using (var cmd = new SqlCommand(SelectAllSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = SqlHelper.ExecSp(cmd);
                return (from DataRow row in ds.Tables[0].Rows select GeneratePlaylist(row));
            }
        }


        public Playlist GetById(int id)
        {
            using (var cmd = new SqlCommand(SelectByIdSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumn, SqlDbType.Int).Value = id;
                DataSet ds = SqlHelper.ExecSp(cmd);
                return ds.Tables[0].Rows.Count == 0 ? null : GeneratePlaylist(ds.Tables[0].Rows[0]);
            }
        }

        public void Add(Playlist pl)
        {
            using (var cmd = new SqlCommand(InsertSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + NameColumn, SqlDbType.NVarChar, 50).Value = pl.Name;
                cmd.Parameters.Add("@" + DescriptionColumn, SqlDbType.NVarChar, -1).Value = pl.Description;
                cmd.Parameters.Add("@" + IdColumn, SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                pl.Id = (int) cmd.Parameters["@" + IdColumn].Value;
            }
        }


        public void Update(Playlist pl)
        {
            using (var cmd = new SqlCommand(UpdateSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumn, SqlDbType.Int).Value = pl.Id;
                cmd.Parameters.Add("@" + NameColumn, SqlDbType.NVarChar, 50).Value = pl.Name;
                cmd.Parameters.Add("@" + DescriptionColumn, SqlDbType.NVarChar, 100).Value = pl.Description;
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(Playlist pl)
        {
            using (var cmd = new SqlCommand(DeleteSp, _con))
            {
                cmd.Parameters.Add("@" + IdColumn, SqlDbType.Int).Value = pl.Id;
                cmd.ExecuteNonQuery();
            }
        }


        public IEnumerable<string> GetSongIds(Playlist pl)
        {
            using (var cmd = new SqlCommand(SelectTracksSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumnPlaylistTracks, SqlDbType.Int).Value = pl.Id;
                DataSet ds = SqlHelper.ExecSp(cmd);
                return (from DataRow row
                            in ds.Tables[0].Rows
                        orderby row[1] ascending
                        select row[0].ToString());
            }
        }

        public void AddSong(Playlist pl, Song song)
        {
            using (var cmd = new SqlCommand(AddTrackSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumnPlaylistTracks, SqlDbType.Int).Value = pl.Id;
                cmd.Parameters.Add("@" + SpotifyIdColumnPlaylistTracks, SqlDbType.NVarChar, 50).Value = song.Id;
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveSong(Playlist pl, Song song)
        {
            using (var cmd = new SqlCommand(RemoveTrackSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumnPlaylistTracks, SqlDbType.Int).Value = pl.Id;
                cmd.Parameters.Add("@" + SpotifyIdColumnPlaylistTracks, SqlDbType.NVarChar, 50).Value = song.Id;
                cmd.ExecuteNonQuery();
            }
        }

        public void MoveSongUp(Playlist pl, Song song)
        {
            using (var cmd = new SqlCommand(MoveSongUpSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumnPlaylistTracks, SqlDbType.Int).Value = pl.Id;
                cmd.Parameters.Add("@" + SpotifyIdColumnPlaylistTracks, SqlDbType.NVarChar, 50).Value = song.Id;
                cmd.ExecuteNonQuery();
            }
        }

        public void MoveSongDown(Playlist pl, Song song)
        {
            using (var cmd = new SqlCommand(MoveSongDownSp, _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@" + IdColumnPlaylistTracks, SqlDbType.Int).Value = pl.Id;
                cmd.Parameters.Add("@" + SpotifyIdColumnPlaylistTracks, SqlDbType.NVarChar, 50).Value = song.Id;
                cmd.ExecuteNonQuery();
            }
        }

        private static Playlist GeneratePlaylist(DataRow row)
        {
            return new Playlist
                {
                    Id = Convert.ToInt32(row[IdColumn].ToString()),
                    Name = row[NameColumn].ToString(),
                    Description = row[DescriptionColumn].ToString()
                };
        }
    }
}