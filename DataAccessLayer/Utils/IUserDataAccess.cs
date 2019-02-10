using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Utils
{
    public interface IUserDataAccess
    {
        List<User> GetAll();
        User Get(int id);
        User Create(User model);
        void Update(User model);
        void Delete(int id);
        User Exists(string username);
    }
}
