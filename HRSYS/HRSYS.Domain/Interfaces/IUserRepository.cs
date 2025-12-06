using HRSYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSYS.Domain.Interfaces
{
    
        public interface IUserRepository
        {
            User? Login(string username);
            User? GetById(int userId);
            IEnumerable<User> ?  GetAll();
            int Insert(User user);
            bool   Update(User user);
            bool  Delete(int userId);
        }
    }

