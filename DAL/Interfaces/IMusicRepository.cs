using System;
using Common.Entities;

namespace DAL.Interfaces
{
    public interface IMusicRepository
    {
        Song GetById(String songId);
    }
}