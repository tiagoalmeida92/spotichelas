using System.Collections.Generic;
using Common.Entities;

namespace DAL.Interfaces
{
    public interface IPlaylistRepository
    {
        //Crud
        void Add(Playlist pl);
        IEnumerable<Playlist> GetAll();
        Playlist GetById(int id);
        void Update(Playlist pl);
        void Delete(Playlist pl);
    }
}