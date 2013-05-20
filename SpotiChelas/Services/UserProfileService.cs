using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Persistence.Repositories;

namespace Services
{
    public class UserProfileService
    {
        public UserProfile GetUser(string userName)
        {
            using (var db = new SpotiChelasDb())
            {
                return db.UserProfiles.FirstOrDefault(profile => profile.UserId == userName);
            }
        }

        public void UpdateUser(UserProfile userProfile)
        {
            using (var db = new SpotiChelasDb())
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void InsertProfile(UserProfile userProfile)
        {
            using (var db = new SpotiChelasDb())
            {
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
            }
        }
    }
}
