using DataAccessLayer.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.Enum.UserRoleEnum;

namespace DataAccessLayer.LocalDataAccess
{

    public static class LocalUserData
    {
        private static List<User> data;
   
        public static List<User> GetData()
        {
            if (data == null)
            {
                data = new List<User>();
            }
            return data;
        }

        public static void Reboot()
        {
            data = new List<User>();
        }

    }

    public class LocalUserDataAccess : IUserDataAccess
    {
        public List<User> GetAll()
        {
            return LocalUserData.GetData();
        }

        public User Get(int id)
        {
            if (LocalUserData.GetData().Where(u => u.ID == id).Count() == 1)
                return LocalUserData.GetData().Where(u => u.ID == id).First();
            else
                return new User();
        }

        public User Create(User model)
        {
            model.ID = LocalUserData.GetData().Count + 1;
            LocalUserData.GetData().Add(model);
            return model;
        }

        public void Update(User model)
        {
            User userToUpdate = Get(model.ID);
            userToUpdate.Password = model.Password;
            userToUpdate.Username = model.Username;
            userToUpdate.Role = model.Role;
        }

        public void Delete(int id)
        {
            LocalUserData.GetData().Remove(Get(id));
        }

        public User Exists(string username)
        {
            if (LocalUserData.GetData().Where(u => u.Username == username).Count() == 1)
                return LocalUserData.GetData().Find(u => u.Username == username);
            else
                return new User();
        }

      
        
    }
}
