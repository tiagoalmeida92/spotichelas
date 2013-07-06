using System;
using System.Data;
using Dto;
using Persistence.DO;
using Persistence.Repositories;
using AutoMapper;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly Db _db;

        public UserService(Db db)
        {
            _db = db;
        }

        public void CreateProfile(string userName)
        {
            _db.UserProfiles.Add(new UserProfile
                {
                    UserId = userName,
                    Name = userName
                });
            _db.SaveChanges();
        }

        public UserProfileDto GetById(string userName)
        {
            var profile = _db.UserProfiles.Find(userName);
            return Mapper.Map<UserProfile, UserProfileDto>(profile);
        }

        public void Update(UserProfileDto dto)
        {
            var profile = Mapper.Map<UserProfile>(dto);
            _db.Entry(profile).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(string id)
        {
            _db.Entry(new UserProfile {UserId = id}).State = EntityState.Deleted;
            _db.SaveChanges();

        } 
    }
}