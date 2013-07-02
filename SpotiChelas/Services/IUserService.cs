using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;

namespace Services
{
    public interface IUserService
    {
        void CreateProfile(string userName);
        UserProfileDto GetById(string userName);
        void Update(UserProfileDto profile);
        void Delete(string id);
    }
}
